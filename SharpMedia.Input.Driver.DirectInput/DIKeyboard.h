#pragma once
#include <windows.h>
#include <dinput.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Input {
namespace Driver {
namespace DirectInput {

	// A direct input keyboard
	public ref class DIKeyboard : public IInputDevice
	{
		IDirectInputDevice8* keyboard;
		char* keys;
	public:
		DIKeyboard(IDirectInputDevice8* key);
		virtual void GetState(array<bool>^ button, array<Int64>^ axis);
		virtual ~DIKeyboard();

	};
}
}
}
}