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
	public ref class DIMouse : public IInputDevice
	{
		IDirectInputDevice8* mouse;
		UInt64 axis1, axis2, axis3;
	public:
		DIMouse(IDirectInputDevice8* key);
        virtual void GetState(array<bool>^ button, array<Int64>^ axis);
		virtual ~DIMouse();
	};
}
}
}
}