#pragma once
#include <windows.h>
#include <D3D10.h>

using namespace System;
using namespace System::Runtime::InteropServices;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	// A Window backend is directly viewed.
	public ref class D3D10WindowBackend : public IWindowBackend
	{
	protected:
		HWND hWnd;
		IWindow^ listener;
	public:
		D3D10WindowBackend(HWND hWnd);
		virtual void SetListener(IWindow^ wnd);
		virtual IWindow^ GetListener();
		virtual property IntPtr Handle { IntPtr get(); };
		virtual void DoEvents();
	};

}
}
}
}