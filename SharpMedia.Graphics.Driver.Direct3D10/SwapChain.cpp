#include "SwapChain.h"
#include "Helper.h"


namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	void D3D10SwapChain::CreateBackRT()
	{
		ID3D10Texture2D *pBackBuffer;
		ID3D10RenderTargetView* pRenderTargetView;

		// We get the bugger.
        if(FAILED(chain->GetBuffer( 0, __uuidof(ID3D10Texture2D), (void**)&pBackBuffer )))
		{
			throw gcnew Exception("Could not extract D3D10 buffer from swap chain.");
		}

		// We create the render target view.
		if(FAILED(device->CreateRenderTargetView(pBackBuffer, NULL, &pRenderTargetView)))
		{
			pBackBuffer->Release();
		}

		backBuffer = pRenderTargetView;
		pBackBuffer->Release();
	}

	D3D10SwapChain::D3D10SwapChain(IDXGISwapChain* chain, ID3D10Device* device)
	{
		this->chain = chain;
		this->device = device;
	
		CreateBackRT();
	}

	void D3D10SwapChain::Resize(UInt32 width, UInt32 height)
	{
		backBuffer->Release();
		backBuffer = 0;

		// We create new descriptor.
		DXGI_SWAP_CHAIN_DESC newMode;
		chain->GetDesc(&newMode);

		if(FAILED(chain->ResizeBuffers(newMode.BufferCount, width, height, DXGI_FORMAT_UNKNOWN, DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH)))
		{
			throw gcnew Exception("Resiing buffers failed.");
		}
		CreateBackRT();
	}

	void D3D10SwapChain::Reset(UInt32 width, UInt32 height, CommonPixelFormatLayout layout, bool fs)
	{


		if(FAILED(chain->SetFullscreenState(fs, NULL)))
		{
			throw gcnew Exception("Failed to switch state.");
		}

		DXGI_MODE_DESC desc;
		desc.Format = ToDXFormat(layout);
		desc.Width = width;
		desc.Height = height;
		desc.RefreshRate.Denominator = 0;
		desc.RefreshRate.Numerator = 0;
		desc.Scaling = DXGI_MODE_SCALING_UNSPECIFIED;
		desc.ScanlineOrdering = DXGI_MODE_SCANLINE_ORDER_PROGRESSIVE;

		if(FAILED(chain->ResizeTarget(&desc)))
		{
			throw gcnew Exception("Failed to resize target.");
		}

		
	}
	
	void D3D10SwapChain::Present()
	{
		chain->Present(0, 0);
	}

	void D3D10SwapChain::Finish()
	{
	}

	void D3D10SwapChain::Clear(ID3D10Device* device, Colour colour)
	{
		float c[] = { colour.R, colour.G, colour.B, colour.A };
		device->ClearRenderTargetView(backBuffer, c);
	}

	D3D10SwapChain::~D3D10SwapChain()
	{
		backBuffer->Release();
		chain->Release();
		chain = 0;
	}



}
}
}
}