#pragma once
#define WINDOWS_LEAN_AND_MEAN
#include <windows.h>
#include <D3D10.h>

using namespace System;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

// A D3D10 Graphics Service Class (shared).	
	public ref class D3D10GraphicsService
{
private:
	bool sharingEnabled;
	ID3D10Device* device;
public:
	D3D10GraphicsService();
	ID3D10Device* CreateDevice(bool shared, RenderTargetParameters^ params, IWindowBackend^% window, ISwapChain^% chain, bool debug);
	ID3D10Device* Get();
	void Nullify();
};

}
}
}
}