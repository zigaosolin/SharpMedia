#pragma once
#include <windows.h>
#include <dinput.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Input {
namespace Driver {
namespace DirectInput {

	// A direct mouse keyboard
	public ref class DICursor : public IInputDevice
	{
		HWND hWnd;
	public:
		DICursor(HWND hWnd);
        virtual void GetState(array<bool>^ button, array<Int64>^ axis);
		virtual ~DICursor();
	};
}
}
}
}