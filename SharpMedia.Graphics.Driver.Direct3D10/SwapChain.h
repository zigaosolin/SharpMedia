#pragma once
#include <windows.h>
#include <D3D10.h>

using namespace System;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	// A swap chain is directly viewed.
	public ref class D3D10SwapChain : public ISwapChain
	{
		IDXGISwapChain* chain;
		ID3D10Device* device;

		void CreateBackRT();
	internal:
		ID3D10RenderTargetView* backBuffer;
	public:
		D3D10SwapChain(IDXGISwapChain* chain, ID3D10Device* device);
		virtual void Present();
		virtual void Finish();
		virtual void Resize(UInt32 width, UInt32 height);
		virtual void Reset(UInt32 width, UInt32 height, CommonPixelFormatLayout layout, bool fs);
		virtual ~D3D10SwapChain();

		void Clear(ID3D10Device* device, Colour colour);
	};

}
}
}
}