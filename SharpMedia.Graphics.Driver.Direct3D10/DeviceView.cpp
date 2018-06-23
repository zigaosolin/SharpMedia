#include <D3D10.h>
#include <map>
#include "DeviceView.h"
#include "Helper.h"
#include "States.h"
#include "Shaders.h"
#include "ShaderCompiler.h"
#include "SwapChain.h"
#include "VerticesBindingLayout.h"
#include "RenderTargetView.h"
#include "DepthStencilTargetView.h"
#include "Buffer.h"
#include "GraphicsService.h"
#include "Texture2d.h"

using namespace System::Collections::Generic;


namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

		D3D10DeviceView::D3D10DeviceView(ID3D10Device* device, D3D10GraphicsService^service)
		{
			this->device = device;
			this->service = service;

			// Extract multithread layer.
			ID3D10Multithread* mt;
			DXFAILED(device->QueryInterface<ID3D10Multithread>(&mt));
			multithread = mt;

			this->sharedTextures = gcnew Collections::Generic::SortedDictionary<Guid, SharedTextureInfo^>();
		}

		SharedTextureInfo^ D3D10DeviceView::GetShared(Guid guid)
		{
			SharedTextureInfo^ value;
			if(this->sharedTextures->TryGetValue(guid, value)) return value;
			return nullptr;
		}

        void D3D10DeviceView::RegisterShared(Guid guid, SharedTextureInfo^ info)
		{
			this->sharedTextures->Add(guid, info);
		}

        void D3D10DeviceView::UnregisterShared(Guid guid)
		{
			this->sharedTextures->Remove(guid);
		}

		UInt64 D3D10DeviceView::DeviceMemory::get()
		{
			// TODO:
			return 0;
		}

		String^ D3D10DeviceView::Name::get()
		{ 
			return "DirectX10 Device";
		}

		void D3D10DeviceView::RegisterListener(IDeviceListener^ listener)
		{
			this->listener = listener;
		}

		void D3D10DeviceView::UnRegisterListener()
		{
			this->listener = nullptr;
		}

		FormatUsage D3D10DeviceView::FormatSupport(CommonPixelFormatLayout fmt)
		{
			UINT data;
			DXGI_FORMAT format;
			try {
				format = ToDXFormat(fmt);
			} catch(Exception^ )
			{
				return FormatUsage::None;
			}
			  
			if(FAILED(device->CheckFormatSupport(format, &data)))
			{
			   throw gcnew Exception("Format support failed.");
			}

// They are somehow not included
#define D3D10_DDI_FORMAT_SUPPORT_SHADER_SAMPLE				0x00000001
#define D3D10_DDI_FORMAT_SUPPORT_RENDERTARGET				0x00000002
#define D3D10_DDI_FORMAT_SUPPORT_BLENDABLE					0x00000004
#define D3D10_DDI_FORMAT_SUPPORT_MULTISAMPLE_RENDERTARGET	0x00000008
			

			int usage = 0;
			if(data & D3D10_DDI_FORMAT_SUPPORT_SHADER_SAMPLE) usage |= (int)FormatUsage::ShaderTexture;
			if(data & D3D10_DDI_FORMAT_SUPPORT_RENDERTARGET) usage |= (int)FormatUsage::RenderTarget;
			if(data & D3D10_DDI_FORMAT_SUPPORT_BLENDABLE) usage |= (int)FormatUsage::BlendRenderTarget;
			if(data & D3D10_DDI_FORMAT_SUPPORT_MULTISAMPLE_RENDERTARGET) usage |= (int)FormatUsage::MultisampleRenderTarget;

			return (FormatUsage)usage;
		}

		unsigned int D3D10DeviceView::MultiSamplingQuality(CommonPixelFormatLayout format, unsigned int sampleCount)
		{
			UINT levels;
			DXFAILED(device->CheckMultisampleQualityLevels(ToDXFormat(format), sampleCount, &levels));
			return levels;
		}

		IBlendState^ D3D10DeviceView::CreateState(States::BlendState^ desc)
		{
			D3D10_BLEND_DESC d;
			d.AlphaToCoverageEnable = desc->AlphaToCoverage;
			for(int i = 0; i < 8; i++)
			{
				d.BlendEnable[i] = desc[i];
				d.RenderTargetWriteMask[i] = ToDXWriteMask(desc->GetWriteMask(i));
			}
			d.BlendOp = ToDXOperation(desc->BlendOperation);
			d.BlendOpAlpha = ToDXOperation(desc->AlphaBlendOperation);
			d.DestBlend = ToDXOperand(desc->BlendDestination);
			d.DestBlendAlpha = ToDXOperand(desc->AlphaBlendDestination);
			d.SrcBlend = ToDXOperand(desc->BlendSource);
			d.SrcBlendAlpha = ToDXOperand(desc->AlphaBlendSource);
			
			ID3D10BlendState* state;
			DXFAILED(device->CreateBlendState(&d, &state));

			return gcnew D3D10BlendState(state);
			
		}


		IRasterizationState^ D3D10DeviceView::CreateState(States::RasterizationState^ desc)
		{
			D3D10_RASTERIZER_DESC d;
			d.FillMode = ToDXMode(desc->FillMode);
			d.CullMode = ToDXMode(desc->CullMode);
			d.FrontCounterClockwise = desc->BackFacing == States::Facing::CCW;
			d.DepthBias = desc->DepthBias;
			d.DepthBiasClamp = desc->DepthBiasClamp;
			d.SlopeScaledDepthBias = desc->ScopeScaledDepthBias;
			d.DepthClipEnable = desc->DepthClipEnabled;
			d.ScissorEnable = desc->ScissorTestEnabled;
			d.MultisampleEnable = desc->MultiSamplingEnabled;
			d.AntialiasedLineEnable = desc->LineAntialisingEnabled;

			ID3D10RasterizerState* state;
			DXFAILED(device->CreateRasterizerState(&d, &state));

			return gcnew D3D10RasterizationState(state);
		}

		IDepthStencilState^ D3D10DeviceView::CreateState(States::DepthStencilState^ desc)
		{
			D3D10_DEPTH_STENCIL_DESC d;
			d.FrontFace.StencilDepthFailOp = ToDXOperation(desc->FrontDepthFail);
			d.FrontFace.StencilFailOp = ToDXOperation(desc->FrontStencilFail);
			d.FrontFace.StencilPassOp = ToDXOperation(desc->FrontDepthPass);
			d.FrontFace.StencilFunc = ToDXFunc(desc->FrontStencilCompare);

			d.BackFace.StencilDepthFailOp = ToDXOperation(desc->BackDepthFail);
			d.BackFace.StencilFailOp = ToDXOperation(desc->BackStencilFail);
			d.BackFace.StencilPassOp = ToDXOperation(desc->BackDepthPass);
			d.BackFace.StencilFunc = ToDXFunc(desc->BackStencilCompare);

			d.DepthEnable = desc->DepthTestEnabled;
			d.DepthFunc = ToDXFunc(desc->DepthCompare);
			d.DepthWriteMask = desc->DepthWriteEnabled ? D3D10_DEPTH_WRITE_MASK_ALL : D3D10_DEPTH_WRITE_MASK_ZERO;
			d.StencilEnable = desc->StencilTestEnabled;
			d.StencilReadMask = desc->StencilReadMask;
			d.StencilWriteMask = desc->StencilWriteMask;

			ID3D10DepthStencilState* state;
			DXFAILED(device->CreateDepthStencilState(&d, &state));

			return gcnew D3D10DepthStencilState(state);
		}
		
		ISamplerState^ D3D10DeviceView::CreateState(States::SamplerState^ desc)
		{
			D3D10_SAMPLER_DESC d;
			d.AddressU = ToDXAddress(desc->AddressU);
			d.AddressV = ToDXAddress(desc->AddressV);
			d.AddressW = ToDXAddress(desc->AddressW);
			d.BorderColor[0] = desc->BorderColour.R;
			d.BorderColor[1] = desc->BorderColour.G;
			d.BorderColor[2] = desc->BorderColour.B;
			d.BorderColor[3] = desc->BorderColour.A;
			d.ComparisonFunc = D3D10_COMPARISON_ALWAYS;
			d.Filter = ToDXFilter(desc->Filter, desc->MipmapFilter);
			d.MipLODBias = desc->MipmapLODBias;
			d.MaxAnisotropy = desc->MaxAnisotropy;
			d.MaxLOD = (FLOAT)desc->MaxMipmap;
			d.MinLOD = (FLOAT)desc->MinMipmap;
			
			ID3D10SamplerState* state;
			DXFAILED(device->CreateSamplerState(&d, &state));

			return gcnew D3D10SamplerState(state);
		}

        IVerticesBindingLayout^ D3D10DeviceView::CreateVertexBinding(array<VertexBindingElement>^ desc)
		{
			// TODO: Add support for matrices.


			// Count number of elements.
			unsigned int length = 0;
			for(int j = 0; j < desc->Length; j++)
			{
				length += desc[j].Format->ElementCount;
			}

			D3D10_INPUT_ELEMENT_DESC* layout = 0;
			try {
				SortedList<PinComponent, VertexFormat::Element^>^ sortedElements
					= gcnew SortedList<PinComponent, VertexFormat::Element^>(length);

				layout = new D3D10_INPUT_ELEMENT_DESC[length];
				int i = 0;
				for(int j = 0; j < desc->Length; j++)
				{
					VertexFormat^ format = desc[j].Format;
				
					// We go through all elements.
					for(unsigned int k = 0; k < format->ElementCount; k++, i++)
					{
						VertexFormat::Element^ element = format[k];
						D3D10_INPUT_ELEMENT_DESC& d3ddesc = layout[i];

						// We fill in the data.
						d3ddesc.SemanticName = ToDXComponent(element->Component, d3ddesc.SemanticIndex);
						d3ddesc.Format = ToDXFormat(element->Format);
						d3ddesc.InputSlot = j;
						d3ddesc.AlignedByteOffset = element->Offset;
						d3ddesc.InputSlotClass = desc[j].UpdateFrequency == UpdateFrequency::PerVertex ?
								D3D10_INPUT_PER_VERTEX_DATA : D3D10_INPUT_PER_INSTANCE_DATA;
						d3ddesc.InstanceDataStepRate = desc[j].UpdateFrequency == UpdateFrequency::PerVertex ? 0 : desc[j].UpdateFrequencyCount;

						// Add to map.
						sortedElements->Add(element->Component, element);
					}
				
				}

				// We have the layout, we can create shader (actually a dummy shader) with it. We always
				// sort parameters by component value (shader generator garantie).
				D3D10ShaderCompiler^ compiler = (D3D10ShaderCompiler^)CreateShaderCompiler();
				compiler->Begin(BindingStage::VertexShader);

				for(int d = 0;d < sortedElements->Count; d++)
				{
					VertexFormat::Element^ input = sortedElements->Values[d];
					compiler->RegisterInput(d, input->Format, input->Component);
				}

				// We end shader compiled to bytecodes.
				ID3D10Blob* blob = 0;
				
				try {
					blob = compiler->EndBytecode();

					// We can now create layout.
					ID3D10InputLayout* verticesLayout = NULL;
					DXFAILED(device->CreateInputLayout(layout, length, blob->GetBufferPointer(),
						blob->GetBufferSize(), &verticesLayout));

					return gcnew D3D10VerticesBindingLayout(verticesLayout);
				} finally
				{
					if(blob) blob->Release();
					
					// We dispose compiler.
					delete compiler;
				}
			} finally {
				delete [] layout;
			}
		}

		void D3D10DeviceView::ClearStates()
		{
			device->ClearState();
		}

        IBuffer^ D3D10DeviceView::CreateBuffer(BufferUsage bufferUsage, Usage usage, CPUAccess access, UInt64 length, array<Byte>^ initialData)
		{
			// Buffer description.
			D3D10_BUFFER_DESC desc;
			desc.ByteWidth = (unsigned int) length;
			desc.Usage = ToDXUsage(usage);
			desc.CPUAccessFlags = ToDXCPUAccess(access);
			desc.MiscFlags = 0;
			desc.BindFlags = ToDXBindFlags(bufferUsage);

			// Buffer data.
			D3D10_SUBRESOURCE_DATA data;
			data.SysMemPitch = 0;
			data.SysMemSlicePitch = 0;
			data.pSysMem = 0;
			
			
			try {

				// Copy data
				if(initialData != nullptr)
				{
					Byte* b = new Byte[(int)length];
					data.pSysMem = b;
					for(UInt64 i = 0; i < length; i++)
					{
						b[i] = initialData[(int)i];
					}
				}

				// Create buffer
				ID3D10Buffer* buffer = 0;
				DXFAILED(device->CreateBuffer(&desc, initialData != nullptr ? &data : 0, &buffer));
				return gcnew D3D10Buffer(buffer);

			} finally {
				delete [] data.pSysMem;
			}
		}

        ITexture1D^ D3D10DeviceView::CreateTexture1D(Usage usage, CommonPixelFormatLayout fmt, CPUAccess access, unsigned int width,
                                         unsigned int mipmapLevels, TextureUsage textureUsage, array<array<Byte>^>^ data)
		{

			return nullptr;
		}

        ITexture2D^ D3D10DeviceView::CreateTexture2D(Usage usage, CommonPixelFormatLayout fmt, CPUAccess access, unsigned int width, unsigned int height,
                                         unsigned int mipmapLevels, TextureUsage textureUsage,
                                         unsigned int sampleCount, unsigned int sampleQuality, array<array<Byte>^>^ initialData)
		{

			// Fill descriptor.
			D3D10_TEXTURE2D_DESC desc;
			desc.ArraySize = 1;
			desc.BindFlags = 0;
			desc.Usage = ToDXUsage(usage);
			desc.MiscFlags = 0;
			desc.CPUAccessFlags = ToDXCPUAccess(access);
			desc.Format = ToDXFormat(fmt);
			desc.Width = width;
			desc.Height = height;
			desc.MipLevels = mipmapLevels;
			desc.SampleDesc.Count = sampleCount;
			desc.SampleDesc.Quality = sampleQuality;

			if((UInt32)textureUsage & (UInt32)TextureUsage::Texture) desc.BindFlags |= D3D10_BIND_SHADER_RESOURCE;
			if((UInt32)textureUsage & (UInt32)TextureUsage::RenderTarget) desc.BindFlags |= D3D10_BIND_RENDER_TARGET;
			if((UInt32)textureUsage & (UInt32)TextureUsage::DepthStencilTarget) desc.BindFlags |= D3D10_BIND_DEPTH_STENCIL;
			if((UInt32)textureUsage & (UInt32)TextureUsage::CubeMap) desc.MiscFlags |= D3D10_RESOURCE_MISC_TEXTURECUBE;

			// We allow mipmap generation if texture & render target.
			if((desc.BindFlags & (D3D10_BIND_SHADER_RESOURCE|D3D10_BIND_RENDER_TARGET)) == (D3D10_BIND_SHADER_RESOURCE|D3D10_BIND_RENDER_TARGET))
			{
				desc.MiscFlags = D3D10_RESOURCE_MISC_GENERATE_MIPS;
			}

			// Fill data.
			D3D10_SUBRESOURCE_DATA* data = 0;

			try {


				// Copy data
				if(initialData != nullptr)
				{
					data = new D3D10_SUBRESOURCE_DATA[initialData->Length];
					memset(data, 0, sizeof(D3D10_SUBRESOURCE_DATA)*initialData->Length);

					for(int i = 0; i < initialData->Length; i++)
					{
						array<Byte>^ src = initialData[i];

						// Compute dimensions.
						UInt32 w = width >> i;
						UInt32 h = height >> i;
						w = w > 0 ? w : 1;
						h = h > 0 ? h : 1;

						Byte* b = new Byte[h * w * ToFormatSize(desc.Format)];
						data[i].pSysMem = b;
						data[i].SysMemPitch = w * ToFormatSize(desc.Format);
						for(Int32 i = 0; i < src->Length; i++)
						{
							b[i] = src[i];
						}
					}
				} 

				ID3D10Texture2D* texture2d;
				if(FAILED(device->CreateTexture2D(&desc, data, &texture2d)))
				{
					throw gcnew Exception("Could not create texture 2D.");
				}

				return gcnew D3D10Texture2d(texture2d);


			} finally {

				if(data != 0)
				{
					for(int i = 0; i < initialData->Length; i++)
					{
						delete [] data[i].pSysMem;
					}

					delete [] data;
				}
			}
		}

        ITexture3D^ D3D10DeviceView::CreateTexture3D(Usage usage, CommonPixelFormatLayout fmt, CPUAccess access, unsigned int width, unsigned int height, 
											unsigned int depth, unsigned int mipmapLevels, TextureUsage textureUsage, array<array<Byte>^>^ data)
		{

			return nullptr;
		}

        IShaderCompiler^ D3D10DeviceView::CreateShaderCompiler()
		{
			return gcnew D3D10ShaderCompiler(device);
		}

        void D3D10DeviceView::Enter()
		{
			multithread->Enter();
		}

        void D3D10DeviceView::Exit()
		{
			multithread->Leave();
		}

        void D3D10DeviceView::Clear(IRenderTargetView^ view, Colour colour)
		{
			if(view->GetType() == D3D10RenderTargetView::typeid)
			{
				D3D10RenderTargetView^ v = (D3D10RenderTargetView^)view;
				v->Clear(device, colour);
			} else {
				D3D10SwapChain^ v = (D3D10SwapChain^)view;
				v->Clear(device, colour);
			}
		}

        void D3D10DeviceView::Clear(IDepthStencilTargetView^ view,
			ClearOptions options, float depth, unsigned int stencil)
		{
			D3D10DepthStencilTargetView^ v = (D3D10DepthStencilTargetView^)view;
			v->Clear(device, options, depth, stencil);
		}

		void D3D10DeviceView::DrawAuto()
		{
			device->DrawAuto();
		}

		void D3D10DeviceView::Draw(UInt64 off, UInt64 length)
		{
			device->Draw((unsigned int)length, (unsigned int)off);
		}

        void D3D10DeviceView::DrawIndexed(UInt64 offset, UInt64 count, Int64 baseIndex,
			unsigned int instanceOffset, unsigned int instanceCount)
		{
			device->DrawIndexedInstanced((unsigned int)count, instanceCount, 
				(unsigned int)offset, (int)baseIndex, instanceOffset);
		}

		void D3D10DeviceView::DrawIndexed(UInt64 off, UInt64 length, Int64 baseIndex)
		{
			device->DrawIndexed((unsigned int)length, (unsigned int)off, (int)baseIndex);
		}

        void D3D10DeviceView::Draw(UInt64 offset, UInt64 count, 
			unsigned int instanceOffset, unsigned int instanceCount)
		{
			device->DrawInstanced((unsigned int)count, instanceCount, 
				(unsigned int)offset, instanceOffset);
		}

        void D3D10DeviceView::BindGStage(IGShader^ gshader, array<ISamplerState^>^ samplers, array<ITextureView^>^ textures,
                        array<ICBufferView^>^ constants, IVerticesOutBindingLayout^ layout, array<IVBufferView^>^ vbuffers)
		{
			
		
		}

        void D3D10DeviceView::BindVStage(Topology topology, IVerticesBindingLayout^ layout, array<IVBufferView^>^ vbuffers, IIBufferView^ ibuffer,
			IVShader^ vshader, array<ISamplerState^>^ samplers, array<ITextureView^>^ textures, 
			array<ICBufferView^>^ constants)
		{
			// Local data.
			int i;
			ID3D10SamplerState** samplerStates = 0;
			ID3D10ShaderResourceView** textureStates = 0;
			ID3D10Buffer** constantBuffers = 0;

			try {

				// Layout
				if(layout)
				{
					D3D10VerticesBindingLayout^ d3dLayout = (D3D10VerticesBindingLayout^)layout;
					d3dLayout->Apply(device);
				}

				// Index buffer.
				if(ibuffer)
				{
					D3D10IBuffer^ view = (D3D10IBuffer^)ibuffer;
					view->Apply(device);
				}

				// Vertex buffers.
				for(i = 0; i < vbuffers->Length; i++)
				{
					D3D10VBuffer^ view = (D3D10VBuffer^)vbuffers[i];
					view->Apply(device, (unsigned int)i);
					
				}

				// We set topology.
				switch(topology)
				{
				case Topology::Line:
					device->IASetPrimitiveTopology(D3D10_PRIMITIVE_TOPOLOGY_LINELIST);
					break;
				case Topology::LineStrip:
					device->IASetPrimitiveTopology(D3D10_PRIMITIVE_TOPOLOGY_LINESTRIP);
					break;
				case Topology::Point:
					device->IASetPrimitiveTopology(D3D10_PRIMITIVE_TOPOLOGY_POINTLIST);
					break;
				case Topology::Triangle:
					device->IASetPrimitiveTopology(D3D10_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
					break;
				case Topology::TriangleStrip:
					device->IASetPrimitiveTopology(D3D10_PRIMITIVE_TOPOLOGY_TRIANGLESTRIP);
					break;
				default:
					NOT_SUPPORTED();

				}
				// Vertex shader.
				if(vshader)
				{
					D3D10VShader^ shader = (D3D10VShader^)vshader;
					shader->Apply(device);
				}

				// Samplers.
				samplerStates = new ID3D10SamplerState*[samplers->Length];
				for(i = 0; i < samplers->Length; i++)
				{
					D3D10SamplerState^ state = (D3D10SamplerState^)samplers[i];
					samplerStates[i] = state ? state->state : 0;
				}

				if(samplers->Length > 0)
				{
					device->VSSetSamplers(0, samplers->Length, samplerStates);
				}

				// Texture
				textureStates = new ID3D10ShaderResourceView*[textures->Length];
				for(i = 0; i < textures->Length; i++)
				{
					textureStates[i] = ((D3D10TextureView^)textures[i])->view;
				}

				if(textures->Length > 0)
				{
					device->VSSetShaderResources(0, textures->Length, textureStates);
				}

				// Constant buffers.
				constantBuffers = new ID3D10Buffer*[constants->Length];
				for(i = 0; i < constants->Length; i++)
				{
					D3D10CBuffer^ buffer = (D3D10CBuffer^)constants[i];
					constantBuffers[i] = buffer ? buffer->GetBuffer() : 0;
				}

				if(constants->Length > 0)
				{
					device->VSSetConstantBuffers(0, constants->Length, constantBuffers);
				}

			} finally {
				delete [] samplerStates;
				delete [] textureStates;
				delete [] constantBuffers;
			}
		
		}

		void D3D10DeviceView::BindPStage(IPShader^ pshader, array<ISamplerState^>^ samplers, array<ITextureView^>^ textures,
                        array<ICBufferView^>^ constants, array<IRenderTargetView^>^ renderTargets, 
						IDepthStencilTargetView^ depthTarget)
		{
			// Local data.
			int i;
			ID3D10SamplerState** samplerStates = 0;
			ID3D10ShaderResourceView** textureStates = 0;
			ID3D10Buffer** constantBuffers = 0;
			ID3D10RenderTargetView** renderTargetViews = 0;

			try {

				// Vertex shader.
				if(pshader)
				{
					D3D10PShader^ shader = (D3D10PShader^)pshader;
					shader->Apply(device);
				}

				// Samplers.
				samplerStates = new ID3D10SamplerState*[samplers->Length];
				for(i = 0; i < samplers->Length; i++)
				{
					D3D10SamplerState^ state = (D3D10SamplerState^)samplers[i];
					samplerStates[i] = state ? state->state : 0;
				}

				if(samplers->Length > 0)
				{
					device->PSSetSamplers(0, samplers->Length, samplerStates);
				}

				// Texture
				textureStates = new ID3D10ShaderResourceView*[textures->Length];
				for(i = 0; i < textures->Length; i++)
				{
					textureStates[i] = ((D3D10TextureView^)textures[i])->view;
				}

				if(textures->Length > 0)
				{
					device->PSSetShaderResources(0, textures->Length, textureStates);
				}

				// Constant buffers.
				constantBuffers = new ID3D10Buffer*[constants->Length];
				for(i = 0; i < constants->Length; i++)
				{
					D3D10CBuffer^ buffer = (D3D10CBuffer^)constants[i];
					constantBuffers[i] = buffer ? buffer->GetBuffer() : 0;
				}

				if(constants->Length > 0)
				{
					device->PSSetConstantBuffers(0, constants->Length, constantBuffers);
				}
				

				// Render targets and depth stencil.
				renderTargetViews = new ID3D10RenderTargetView*[renderTargets->Length];
				for(int i = 0; i < renderTargets->Length; i++)
				{
					if(renderTargets[i]->GetType() == D3D10RenderTargetView::typeid)
					{
						D3D10RenderTargetView^ view = (D3D10RenderTargetView^)renderTargets[i];
						renderTargetViews[i] = view->view;
					} else {
						D3D10SwapChain^ view = (D3D10SwapChain^)renderTargets[i];
						renderTargetViews[i] = view->backBuffer;
					}
				}

				D3D10DepthStencilTargetView^ dsView = (D3D10DepthStencilTargetView^)depthTarget;

				device->OMSetRenderTargets(renderTargets->Length, renderTargetViews,
					dsView ? dsView->view : 0);


			} finally {
				delete [] samplerStates;
				delete [] textureStates;
				delete [] constantBuffers;
				delete [] renderTargetViews;
			}
		}


		void D3D10DeviceView::SetViewports(array<Region2i>^ rects)
		{
			D3D10_VIEWPORT* viewports = new D3D10_VIEWPORT[rects->Length];
			for(int i = 0; i < rects->Length; i++)
			{
				D3D10_VIEWPORT& view = viewports[i];
				view.TopLeftX = rects[i].X;
				view.TopLeftY = rects[i].Y;
				view.Width = rects[i].Width;
				view.Height = rects[i].Height;
				view.MinDepth = 0.0f;
				view.MaxDepth = 1.0f;
			}

			device->RSSetViewports(rects->Length, viewports);
			
			delete[] viewports;
			
		}

		void D3D10DeviceView::SetScissorRects(array<Region2i>^ rects)
		{
			D3D10_RECT* viewports = 0;
			try {
				viewports = new D3D10_RECT[rects->Length];
				for(int i = 0; i < rects->Length; i++)
				{
					D3D10_RECT& view = viewports[i];
					view.bottom = rects[i].Y;
					view.top = rects[i].Y + rects[i].Y+rects[i].Height;
					view.left = rects[i].X;
					view.right = rects[i].X + rects[i].Width;
				}

				device->RSSetScissorRects(rects->Length, viewports);
			} finally {
				delete[] viewports;
			}
		}

		void D3D10DeviceView::SetBlendState(IBlendState^ state, Colour colour, unsigned int mask)
		{
			D3D10BlendState^ s = (D3D10BlendState^)state;
			s->Apply(device, colour, mask);
		}
		void D3D10DeviceView::SetDepthStencilState(IDepthStencilState^ state, unsigned int stencilRef)
		{
			D3D10DepthStencilState^ s = (D3D10DepthStencilState^)state;
			s->Apply(device, stencilRef);
		}

		void D3D10DeviceView::SetRasterizationState(IRasterizationState^ state)
		{
			D3D10RasterizationState^ s = (D3D10RasterizationState^)state;
			s->Apply(device);
		}

		ICBufferView^ D3D10DeviceView::CreateCBufferView(IBuffer^ buffer)
		{
			D3D10Buffer^ b = (D3D10Buffer^)buffer;
			return b->CreateCView(device);
		}
		IVBufferView^ D3D10DeviceView::CreateVBufferView(IBuffer^ buffer, unsigned int stride, UInt64 offset)
		{
			D3D10Buffer^ b = (D3D10Buffer^)buffer;
			return b->CreateVView(device, stride, offset);
		}
		IIBufferView^ D3D10DeviceView::CreateIBufferView(IBuffer^ buffer, bool wide, UInt64 offset)
		{
			D3D10Buffer^ b = (D3D10Buffer^)buffer;
			return b->CreateIView(device, wide, offset);
		}
        IRenderTargetView^ D3D10DeviceView::CreateRenderTargetView(Object^ resource, UsageDimensionType usageType, 
			CommonPixelFormatLayout layout, UInt64 param1, UInt64 param2, UInt64 param3)
		{
			D3D10_RENDER_TARGET_VIEW_DESC desc;
			desc.Format = ToDXFormat(layout);
			ID3D10Resource* r;
			
			// If texture2D, we can have 2D or 2DMS views.
			if(resource->GetType() == D3D10Texture2d::typeid)
			{
				if(usageType == UsageDimensionType::Texture2D)
				{
					desc.ViewDimension = D3D10_RTV_DIMENSION_TEXTURE2D;
					desc.Texture2D.MipSlice = (UINT)param1;
				} else if(usageType == UsageDimensionType::Texture2DMS)
				{
					desc.ViewDimension = D3D10_RTV_DIMENSION_TEXTURE2DMS;
				} else {
					throw gcnew NotSupportedException();
				}

				r = ((D3D10Texture2d^)resource)->texture2D;
			} else {
				throw gcnew NotSupportedException();
			}

			// We create resource.
			ID3D10RenderTargetView* view;
			if(FAILED(device->CreateRenderTargetView(r, &desc, &view)))
			{
				throw gcnew Exception("View creation failed.");
			}

			return gcnew D3D10RenderTargetView(view);
		}

        IDepthStencilTargetView^ D3D10DeviceView::CreateDepthStencilTargetView(ITexture^ resource, UsageDimensionType usageType,
			CommonPixelFormatLayout layout, UInt64 param1, UInt64 param2, UInt64 param3)
		{
			D3D10_DEPTH_STENCIL_VIEW_DESC desc;
			desc.Format = ToDXFormat(layout);
			ID3D10Resource* r;
			
			// If texture2D, we can have 2D or 2DMS views.
			if(resource->GetType() == D3D10Texture2d::typeid)
			{
				if(usageType == UsageDimensionType::Texture2D)
				{
					desc.ViewDimension = D3D10_DSV_DIMENSION_TEXTURE2D;
					desc.Texture2D.MipSlice = (UINT)param1;
				} else if(usageType == UsageDimensionType::Texture2DMS)
				{
					desc.ViewDimension = D3D10_DSV_DIMENSION_TEXTURE2DMS;
				} else {
					throw gcnew NotSupportedException();
				}

				r = ((D3D10Texture2d^)resource)->texture2D;
			} else {
				throw gcnew NotSupportedException();
			}

			// We create resource.
			ID3D10DepthStencilView* view;
			if(FAILED(device->CreateDepthStencilView(r, &desc, &view)))
			{
				throw gcnew Exception("View creation failed.");
			}

			return gcnew D3D10DepthStencilTargetView(view);
		}

        ITextureView^ D3D10DeviceView::CreateTextureView(Object^ resource, UsageDimensionType usageType,
			CommonPixelFormatLayout layout, UInt64 param1, UInt64 param2, UInt64 param3)
		{
			if(usageType == UsageDimensionType::Texture2D)
			{
				// We create descriptor.
				D3D10_SHADER_RESOURCE_VIEW_DESC desc;
				desc.Format = ToDXFormat(layout);
				desc.ViewDimension = D3D10_SRV_DIMENSION_TEXTURE2D;
				desc.Texture2D.MipLevels = (UINT)param2;
				desc.Texture2D.MostDetailedMip = (UINT)param1;
				

				// We need to extract resource.
				ID3D10Resource* res = 0;
				

				// We now create view.
				if(resource->GetType() == D3D10Texture2d::typeid)
				{
					res = ((D3D10Texture2d^)resource)->texture2D;
				} else {
					throw gcnew NotImplementedException();
				}

				ID3D10ShaderResourceView* view;
				if(FAILED(device->CreateShaderResourceView(res, &desc, &view)))
				{
					throw gcnew Exception("Could not create texture view.");
				}

				return gcnew D3D10TextureView(view);
			} else if(usageType == UsageDimensionType::Buffer)
			{
				// We create descriptor.
				D3D10_SHADER_RESOURCE_VIEW_DESC desc;
				desc.Format = ToDXFormat(layout);
				desc.ViewDimension = D3D10_SRV_DIMENSION_BUFFER;
				desc.Buffer.ElementOffset = (UINT)param1;
				desc.Buffer.ElementWidth = (UINT)param2;

				// We need to extract resource.
				ID3D10Resource* res = 0;
				

				// We now create view.
				if(resource->GetType() == D3D10Buffer::typeid)
				{
					res = ((D3D10Buffer^)resource)->buffer;
				} else {
					throw gcnew NotImplementedException();
				}

				ID3D10ShaderResourceView* view;
				if(FAILED(device->CreateShaderResourceView(res, &desc, &view)))
				{
					throw gcnew Exception("Could not create texture view.");
				}

				return gcnew D3D10TextureView(view);
			}

			throw gcnew NotImplementedException();
		}

		D3D10DeviceView::~D3D10DeviceView()
		{
			device->Release();
			device = 0;

			service->Nullify();
		}

}
}
}
}