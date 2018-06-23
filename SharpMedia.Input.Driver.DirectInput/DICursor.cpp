
#include "dicursor.h"


namespace SharpMedia {
namespace Input {
namespace Driver {
namespace DirectInput {

	DICursor::DICursor(HWND hwnd)
	{
		this->hWnd = hwnd;
	}

	void DICursor::GetState(array<bool>^ button, array<Int64>^ axis)
	{
		POINT point;
		if(!GetCursorPos(&point)) return;

		// We now clamp it to relative window.
		RECT windowRect;
		if(!GetWindowRect(hWnd, &windowRect)) return;

		// We now clamp.
		if(point.x < windowRect.left) point.x = windowRect.left;
		if(point.y < windowRect.top) point.y =  windowRect.top;
		if(point.x > windowRect.right) point.x = windowRect.right;
		if(point.y > windowRect.bottom) point.y = windowRect.bottom;

		axis[0] = point.x - windowRect.left;
		axis[1] = windowRect.bottom  - point.y;
	}
	
	DICursor::~DICursor()
	{
	}

}
}
}
}