#include "dikeyboard.h"
#include "dimouse.h"
#include "diinput.h"
#include "dicursor.h"

namespace SharpMedia {
namespace Input {
namespace Driver {
namespace DirectInput {


	DIInput::DIInput()
	{
	}

	void DIInput::Initialize(Graphics::Window^ window)
	{
		// We get handle of hosting process.
		HINSTANCE hInstance = GetModuleHandle(NULL);
		IDirectInput8* inp;

		// We create input device.
		if(FAILED(DirectInput8Create(hInstance, DIRECTINPUT_VERSION, IID_IDirectInput8, (void**)&inp, NULL)))
		{
			throw gcnew Exception("Input device creation failed.");
		}
		input = inp;
		hWnd = (HWND)window->WindowHandle.ToPointer();

		// We also initialize descriptor.
		desc = gcnew array<InputDeviceDescriptor^>(3);
		desc[0] = gcnew InputDeviceDescriptor(InputDeviceType::Mouse, "System Mouse", 0, 8, 3);
		desc[1] = gcnew InputDeviceDescriptor(InputDeviceType::Keyboard, "System Keyboard", 0, 256, 0);
		desc[2] = gcnew InputDeviceDescriptor(InputDeviceType::Cursor, "OS Cursor (for matching)", 0, 0, 2);

		// TODO: add other devices
	}

	String^ DIInput::Name::get()
	{
		return gcnew String("DirectInput");
	}

	array<InputDeviceDescriptor^>^ DIInput::SupportedDevices::get()
	{
		return desc;
	}

	IInputDevice^ DIInput::Create(InputDeviceDescriptor^ desc)
	{
		if(desc->DeviceId == 0 && desc->DeviceType == InputDeviceType::Mouse)
		{
			// We register mouse.
			IDirectInputDevice8* m;
			if(FAILED(input->CreateDevice(GUID_SysMouse, &m, NULL)))
			{
				throw gcnew Exception("Mouse device vould not be created.");
			}	

			// We set data.
			if(FAILED(m->SetDataFormat(&c_dfDIMouse2)))
			{
				m->Release();
				throw gcnew Exception("Could not set data format.");
			}

			// We set cooperative level.
			if(FAILED(m->SetCooperativeLevel(hWnd, 
				DISCL_BACKGROUND | DISCL_NONEXCLUSIVE)))
			{
				m->Release();
				throw gcnew Exception("Could not set cooperative level of keyboard");
			}
			
			// We aquire device.
			m->Acquire();

			return gcnew DIMouse(m);
		} else if(desc->DeviceId == 0 && desc->DeviceType == InputDeviceType::Keyboard)
		{
			// Now we register keyboard.
			IDirectInputDevice8* key;
			if(FAILED(input->CreateDevice(GUID_SysKeyboard, &key, NULL)))
			{
				throw gcnew Exception("Keyboard device could not be created.");
			}

			// We set data.
			if(FAILED(key->SetDataFormat(&c_dfDIKeyboard)))
			{
				key->Release();
				throw gcnew Exception("Could not set data format.");
			}

			// We set cooperative level.
			if(FAILED(key->SetCooperativeLevel(hWnd, DISCL_BACKGROUND | DISCL_NONEXCLUSIVE)))
			{
				key->Release();
				throw gcnew Exception("Could not set cooperative level of keyboard");
			}
			
			// We aquire device.
			key->Acquire();

			return gcnew DIKeyboard(key);
		} else if(desc->DeviceId == 0 && desc->DeviceType == InputDeviceType::Cursor)
		{
			return gcnew DICursor(hWnd);
		}

		throw gcnew NotSupportedException();
	}

	DIInput::~DIInput()
	{
		input->Release();
	}
}
}
}
}