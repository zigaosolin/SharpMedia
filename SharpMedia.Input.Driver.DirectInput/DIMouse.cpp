
#include "dimouse.h"


namespace SharpMedia {
namespace Input {
namespace Driver {
namespace DirectInput {

	DIMouse::DIMouse(IDirectInputDevice8* m)
	{
		mouse = m;
	}

	void DIMouse::GetState(array<bool>^ button, array<Int64>^ axis)
	{
		DIMOUSESTATE2 state2;

		// We get state.
		if(FAILED(mouse->GetDeviceState(sizeof(state2), &state2)))
		{
			// We must acquire it.
			if(FAILED(mouse->Acquire()))
			{
				return;
			}

			if(FAILED(mouse->GetDeviceState(sizeof(state2), &state2)))
			{
				throw gcnew Exception("Input could not be aquired.");
			}
		}

		// We have valid date, we copy it to our array (arrays match).
		for(int i = 0; i < 8; i++)
		{
			button[i] = (state2.rgbButtons[i] & 0x80) ? true : false;
		}

		axis[0] = axis1 + state2.lX;
		axis[1] = axis2 + state2.lY;
		axis[2] = axis3 + state2.lZ;

		axis1 += state2.lX;
		axis2 += state2.lY;
		axis3 += state2.lZ;
	}
	
	DIMouse::~DIMouse()
	{
		mouse->Unacquire();
		mouse->Release();
	}

}
}
}
}