#include <D3D10.h>
#include "GraphicsService.h"
#include "Helper.h"
#include "WindowBackend.h"
#include "SwapChain.h"


namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	// Defined in window backend.
	extern LRESULT WINAPI WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);

	D3D10GraphicsService::D3D10GraphicsService()
	{
		device = 0;
		sharingEnabled = false;
	}

	ID3D10Device* D3D10GraphicsService::CreateDevice(bool shared, RenderTargetParameters^ params, IWindowBackend^% window, ISwapChain^% chain, bool debug)
	{
		if(device != 0)
		{
			throw gcnew InvalidOperationException("Cannot create new device; it was previously " +
				"created (possibly in explicit mode).");
		}

		HINSTANCE instance = GetModuleHandle(NULL);

		// Register window classex.
		WNDCLASSEX wcex;
		wcex.cbSize = sizeof(WNDCLASSEX);
		wcex.style          = CS_HREDRAW | CS_VREDRAW;
		wcex.lpfnWndProc    = WndProc;
		wcex.cbClsExtra     = 0;
		wcex.cbWndExtra     = 0;
		wcex.hInstance      = instance;
		wcex.hIcon          = NULL;
		wcex.hCursor        = LoadCursor(NULL, IDC_ARROW);
		wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
		wcex.lpszMenuName   = NULL;
		wcex.lpszClassName  = L"D3D10WindowClass";
		wcex.hIconSm        = NULL;
		if( !RegisterClassEx(&wcex) )
		{
			// Silently ignore: this means that window was already registered on previous
			// device initialization.
			//throw gcnew Exception("Window registration failed.");
		}

		// Create window.
		
		RECT rc = { 0, 0, params->BackBufferWidth, params->BackBufferHeight };
		AdjustWindowRect( &rc, WS_OVERLAPPEDWINDOW, FALSE );
		HWND hWnd = CreateWindow( L"D3D10WindowClass", L"D3D10 Window", WS_OVERLAPPEDWINDOW,
							   CW_USEDEFAULT, CW_USEDEFAULT, rc.right - rc.left, rc.bottom - rc.top, NULL, NULL,
							   instance, NULL);

		if(!hWnd)
		{
			throw gcnew Exception("Window creation failed");
		}

		ShowWindow( hWnd, SW_SHOW );

		ShowCursor(FALSE);

		// Create device & swap chain.

		// Descriptor first.
		DXGI_SWAP_CHAIN_DESC swapChainDesc;
		swapChainDesc.BufferCount = params->BackBufferCount;
		swapChainDesc.Windowed = params->Windowed;
		swapChainDesc.OutputWindow = hWnd;
		swapChainDesc.BufferDesc.Format = ToDXFormat(params->FormatCommon);
		swapChainDesc.BufferDesc.Height = params->BackBufferHeight;
		swapChainDesc.BufferDesc.Width = params->BackBufferWidth;
		swapChainDesc.BufferDesc.RefreshRate.Numerator = params->RefreshRate;
		swapChainDesc.BufferDesc.RefreshRate.Denominator = 1;
		swapChainDesc.BufferDesc.Scaling = DXGI_MODE_SCALING_UNSPECIFIED;
		swapChainDesc.BufferDesc.ScanlineOrdering = DXGI_MODE_SCANLINE_ORDER_PROGRESSIVE;
		swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
		swapChainDesc.Flags = DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH;
		swapChainDesc.SampleDesc.Count = params->MultiSampleType;
		swapChainDesc.SampleDesc.Quality = params->MultiSampleQuality;
		swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;


		// If it is present, override default settings
		IDXGIFactory *pDXGIFactory;

		// We craete device.
		if(FAILED(CreateDXGIFactory(__uuidof(IDXGIFactory), (void**)&pDXGIFactory)))
		{
			throw gcnew Exception("DX10 factory creation failed.");
		}

		// Search for a PerfHUD adapter.
		/*
		UINT nAdapter = 0;
		IDXGIAdapter* adapter = NULL;
		IDXGIAdapter* selectedAdapter = NULL;
		D3D10_DRIVER_TYPE driverType = D3D10_DRIVER_TYPE_HARDWARE;
		while (pDXGIFactory->EnumAdapters(nAdapter, &adapter) != DXGI_ERROR_NOT_FOUND)
		{
			if (adapter)
			{
				DXGI_ADAPTER_DESC adaptDesc;
				if (SUCCEEDED(adapter->GetDesc(&adaptDesc)))
				{
					const bool isPerfHUD = wcscmp(adaptDesc.Description, L"NVIDIA PerfHUD") == 0;
					// Select the first adapter in normal circumstances or the PerfHUD one if it exists.
					if(nAdapter == 0 || isPerfHUD)
						selectedAdapter = adapter;
					if(isPerfHUD)
						driverType = D3D10_DRIVER_TYPE_REFERENCE;
				}
			}
			++nAdapter;
		}*/

		// We create device.
		IDXGISwapChain* swapChain;
		ID3D10Device* pDevice;
		if(FAILED(D3D10CreateDeviceAndSwapChain(0,  debug ? D3D10_DRIVER_TYPE_REFERENCE : D3D10_DRIVER_TYPE_HARDWARE, 0, 
			debug ? D3D10_CREATE_DEVICE_DEBUG : 0, D3D10_SDK_VERSION,
									&swapChainDesc, &swapChain, &pDevice)))
		{	
			throw gcnew Exception("Device creation failed.");
		}

		device = pDevice;
		
		window = gcnew D3D10WindowBackend(hWnd);
		chain = gcnew D3D10SwapChain(swapChain, device);

		return device;
	}

	ID3D10Device* D3D10GraphicsService::Get()
	{
		if(device != 0 && !sharingEnabled)
		{
			return 0;
		}
		return device;
	}

	void D3D10GraphicsService::Nullify()
	{
		if(device != 0)
		{
			device->Release();
			device = 0;
		}
	}

}
}
}
}