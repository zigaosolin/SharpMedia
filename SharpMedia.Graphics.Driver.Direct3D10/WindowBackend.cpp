#include "WindowBackend.h"


namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	

	// Static instance.
	IntPtr current = IntPtr::Zero;

	// A Window procedure.
	LRESULT WINAPI WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
	{
		

		switch(msg)
		{
		case WM_CLOSE:
			{
				GCHandle handle = GCHandle::FromIntPtr(current);
				D3D10WindowBackend^ c = (D3D10WindowBackend^)handle.Target;
				c->GetListener()->Closed();	
			} 
			break;
		case WM_SIZE:
			{
				if(current != IntPtr::Zero)
				{
					GCHandle handle = GCHandle::FromIntPtr(current);
					D3D10WindowBackend^ c = (D3D10WindowBackend^)handle.Target;
					c->GetListener()->Resized(LOWORD(lParam), HIWORD(lParam));
				}
			}
			break;
		case WM_CREATE:
			break;
		case WM_DESTROY:
			{
				GCHandle handle = GCHandle::FromIntPtr(current);
				handle.Free();
				current = IntPtr::Zero; // No more current.

			}
			break;
		}
		return DefWindowProc(hwnd, msg, wParam, lParam);
	}

	D3D10WindowBackend::D3D10WindowBackend(HWND h)
	{
		hWnd = h;
	}
	
	void D3D10WindowBackend::SetListener(IWindow^ wnd)
	{
		listener = wnd;

		// We can set it now.
		if(listener == nullptr)
		{
			current = IntPtr::Zero;
		} else {
			GCHandle handle = GCHandle::Alloc(this);
			current = GCHandle::ToIntPtr(handle);
		}
	}

	IntPtr D3D10WindowBackend::Handle::get()
	{
		return IntPtr(hWnd);
	}

	IWindow^ D3D10WindowBackend::GetListener()
	{
		return listener;
	}

	void D3D10WindowBackend::DoEvents()
	{
		MSG msg;
		while(PeekMessage(&msg, hWnd, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

}
}
}
}