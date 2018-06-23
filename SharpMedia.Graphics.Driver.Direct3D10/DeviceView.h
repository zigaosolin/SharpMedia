#pragma once
#include <windows.h>
#include <D3D10.h>
#include "GraphicsService.h"

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	public ref class D3D10DeviceView : public IDevice
	{
		ID3D10Device* device;
		ID3D10Multithread* multithread;
		IDeviceListener^ listener;
		D3D10GraphicsService^ service;

		Collections::Generic::SortedDictionary<Guid, SharedTextureInfo^>^ sharedTextures;
	public:
		D3D10DeviceView(ID3D10Device* device, D3D10GraphicsService^ service);

        virtual property String^ Name
		{ 
			String^ get();
		}

		virtual property UInt64 DeviceMemory
		{
			UInt64 get();
		}

		virtual SharedTextureInfo^ GetShared(Guid guid);
        virtual void RegisterShared(Guid guid, SharedTextureInfo^ info);
        virtual void UnregisterShared(Guid guid);
		virtual void ClearStates();
        virtual void RegisterListener(IDeviceListener^ listener);
        virtual void UnRegisterListener();
        virtual FormatUsage FormatSupport(CommonPixelFormatLayout fmt);
        virtual unsigned int MultiSamplingQuality(CommonPixelFormatLayout format, unsigned int sampleCount);
		virtual IBlendState^ CreateState(States::BlendState^ desc);
		virtual IRasterizationState^ CreateState(States::RasterizationState^ desc);
		virtual IDepthStencilState^ CreateState(States::DepthStencilState^ desc);
		virtual ISamplerState^ CreateState(States::SamplerState^ desc);
        virtual IVerticesBindingLayout^ CreateVertexBinding(array<VertexBindingElement>^ desc);
        virtual IBuffer^ CreateBuffer(BufferUsage bufferUsage, Usage usage, CPUAccess access, UInt64 length, array<Byte>^ initialData);
        virtual ITexture1D^ CreateTexture1D(Usage usage, CommonPixelFormatLayout fmt, CPUAccess access, unsigned int width,
                                         unsigned int mipmapLevels, TextureUsage textureUsage, array<array<Byte>^>^ data);
        virtual ITexture2D^ CreateTexture2D(Usage usage, CommonPixelFormatLayout fmt, CPUAccess access, unsigned int width, unsigned int height,
                                         unsigned int mipmapLevels, TextureUsage textureUsage,
                                         unsigned int sampleCount, unsigned int sampleQuality, array<array<Byte>^>^ data);

        virtual ITexture3D^ CreateTexture3D(Usage usage, CommonPixelFormatLayout fmt, CPUAccess access, unsigned int width, unsigned int height, 
											unsigned int depth, unsigned int mipmapLevels, TextureUsage textureUsage, array<array<Byte>^>^ data);

        virtual IShaderCompiler^ CreateShaderCompiler();
        virtual void Enter();
        virtual void Exit();
		virtual void Clear(IRenderTargetView^ view, Colour colour);
        virtual void Clear(IDepthStencilTargetView^ view, ClearOptions options, float depth, unsigned int stencil); 
        virtual void DrawAuto();
        virtual void Draw(UInt64 off, UInt64 lenght);
        virtual void Draw(UInt64 offset, UInt64 count,
                  unsigned int instanceOffset, unsigned int instanceCount);
        virtual void DrawIndexed(UInt64 off, UInt64 lenght, Int64 baseVertex);
        virtual void DrawIndexed(UInt64 offset, UInt64 count, Int64 baseVertex,
                  unsigned int instanceOffset, unsigned int instanceCount);

        virtual void BindVStage(Topology topology, IVerticesBindingLayout^ layout, array<IVBufferView^>^ vbuffers, IIBufferView^ ibuffer,
			IVShader^ vshader, array<ISamplerState^>^ samplers, array<ITextureView^>^ textures, 
                        array<ICBufferView^>^ constants);

		virtual void BindGStage(IGShader^ gshader, array<ISamplerState^>^ samplers, array<ITextureView^>^ textures,
                        array<ICBufferView^>^ constants, IVerticesOutBindingLayout^ layout, array<IVBufferView^>^ vbuffers);

		virtual void BindPStage(IPShader^ pshader, array<ISamplerState^>^ samplers, array<ITextureView^>^ textures,
                        array<ICBufferView^>^ constants, array<IRenderTargetView^>^ renderTargets, 
                        IDepthStencilTargetView^ depthTarget);


        virtual void SetViewports(array<Region2i>^ rects);
        virtual void SetScissorRects(array<Region2i>^ rects);
        virtual void SetBlendState(IBlendState^ state, Colour colour, unsigned int mask);
        virtual void SetDepthStencilState(IDepthStencilState^ state, unsigned int stencilRef);
        virtual void SetRasterizationState(IRasterizationState^ state);

        virtual ICBufferView^ CreateCBufferView(IBuffer^ buffer);
        virtual IVBufferView^ CreateVBufferView(IBuffer^ buffer, unsigned int stride, UInt64 offset);
        virtual IIBufferView^ CreateIBufferView(IBuffer^ buffer, bool wide, UInt64 offset);
        virtual IRenderTargetView^ CreateRenderTargetView(Object^ resource, UsageDimensionType usageType, 
                     CommonPixelFormatLayout layout, UInt64 param1, UInt64 param2, UInt64 param3);
        virtual IDepthStencilTargetView^ CreateDepthStencilTargetView(ITexture^ texture, UsageDimensionType usageType,
                    CommonPixelFormatLayout layout, UInt64 param1, UInt64 param2, UInt64 param3);
        virtual ITextureView^ CreateTextureView(Object^ resource, UsageDimensionType usageType,
                     CommonPixelFormatLayout layout, UInt64 param1, UInt64 param2, UInt64 param3);


		virtual ~D3D10DeviceView();

	};

}
}
}
}