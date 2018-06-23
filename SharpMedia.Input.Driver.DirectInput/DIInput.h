#pragma once
#include <windows.h>
#include <dinput.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Input {
namespace Driver {
namespace DirectInput {

	// A direct input provider.
	public ref class DIInput : public IInputService
	{
		HWND hWnd;
		IDirectInput8* input;
		array<InputDeviceDescriptor^>^ desc;
	public:
		DIInput();
		virtual void Initialize(Graphics::Window^ window);
		virtual property String^ Name { String^ get(); }
        virtual property array<InputDeviceDescriptor^>^ SupportedDevices 
		{ 
			array<InputDeviceDescriptor^>^ get(); 
		}
        virtual IInputDevice^ Create(InputDeviceDescriptor^ desc);
		virtual ~DIInput();

	};
}
}
}
}