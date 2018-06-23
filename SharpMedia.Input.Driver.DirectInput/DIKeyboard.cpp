
#include "dikeyboard.h"


namespace SharpMedia {
namespace Input {
namespace Driver {
namespace DirectInput {

	DIKeyboard::DIKeyboard(IDirectInputDevice8* key)
	{
		keyboard = key;
		keys = new char[256];
	}

	static int KeyMapper[256] = 
	{
		0,
		DIK_0,
		DIK_1,
		DIK_2,
		DIK_3,
		DIK_4,
		DIK_5,
		DIK_6,
		DIK_7,
		DIK_8,
		DIK_9,
		DIK_A,
		DIK_ABNT_C1,
		DIK_ABNT_C2,
		DIK_ADD,
        DIK_APOSTROPHE,
        DIK_APPS,
        DIK_AT,
        DIK_AX,
        DIK_B,
        DIK_BACK,
        DIK_BACKSLASH,
        DIK_C,
        DIK_CALCULATOR,
        DIK_CAPITAL,
        DIK_COLON,
		DIK_COMMA,
        DIK_CONVERT,
        DIK_D,
        DIK_DECIMAL,
        DIK_DELETE,
        DIK_DIVIDE,
        DIK_DOWN,
        DIK_E,
        DIK_END,
        DIK_EQUALS,
        DIK_ESCAPE,
        DIK_F,
        DIK_F1,
        DIK_F2,
        DIK_F3,
        DIK_F4,
        DIK_F5,
        DIK_F6,
        DIK_F7,
        DIK_F8,
        DIK_F9,
        DIK_F10,
        DIK_F11,
        DIK_F12,
        DIK_F13,
        DIK_F14,
        DIK_F15,
        DIK_G,
        DIK_GRAVE,
        DIK_H,
        DIK_HOME,
        DIK_I,
        DIK_INSERT,
        DIK_J,
        DIK_K,
        DIK_KANA,
        DIK_KANJI,
        DIK_L,
        DIK_LBRACKET,
        DIK_LCONTROL,
        DIK_LEFT,
        DIK_LMENU,
        DIK_LSHIFT,
        DIK_LWIN,
        DIK_M,
        DIK_MAIL,
        DIK_MEDIASELECT,
        DIK_MEDIASTOP,
        DIK_MINUS,
        DIK_MULTIPLY,
        DIK_MUTE,
        DIK_MYCOMPUTER,
        DIK_N,
        DIK_NEXT,
        DIK_NEXTTRACK,
        DIK_NOCONVERT,
        DIK_NUMLOCK,
        DIK_NUMPAD0,
        DIK_NUMPAD1,
        DIK_NUMPAD2,
        DIK_NUMPAD3,
        DIK_NUMPAD4,
        DIK_NUMPAD5,
        DIK_NUMPAD6,
        DIK_NUMPAD7,
        DIK_NUMPAD8,
        DIK_NUMPAD9,
        DIK_NUMPADCOMMA,
        DIK_NUMPADENTER,
        DIK_NUMPADEQUALS,
        DIK_O,
        DIK_OEM_102,
        DIK_P,
        DIK_PAUSE,
        DIK_PERIOD,
        DIK_PLAYPAUSE,
        DIK_POWER,
        DIK_PREVTRACK,
        DIK_PRIOR,
        DIK_Q,
        DIK_R,
        DIK_RBRACKET,
        DIK_RCONTROL,
        DIK_RETURN,
        DIK_RIGHT,
        DIK_RMENU,
        DIK_RSHIFT,
        DIK_RWIN,
        DIK_S,
        DIK_SCROLL,
        DIK_SEMICOLON,
        DIK_SLASH,
        DIK_SLEEP,
        DIK_SPACE,
        DIK_STOP,
        DIK_SUBTRACT,
        DIK_SYSRQ,
        DIK_T,
        DIK_TAB,
        DIK_U,
        DIK_UNDERLINE,
        DIK_UNLABELED,
        DIK_UP,
        DIK_V,
        DIK_VOLUMEDOWN,
        DIK_VOLUMEUP,
        DIK_W,
        DIK_WAKE,
        DIK_WEBBACK,
        DIK_WEBFAVORITES,
        DIK_WEBFORWARD,
        DIK_WEBHOME,
        DIK_WEBREFRESH,
        DIK_WEBSEARCH,
        DIK_WEBSTOP,
        DIK_X,
        DIK_Y,
        DIK_YEN,
		DIK_Z
	};

	void DIKeyboard::GetState(array<bool>^ button, array<Int64>^ axis)
	{
		// We get state.
		if(FAILED(keyboard->GetDeviceState(256, keys)))
		{
			// We must acquire it.
			if(FAILED(keyboard->Acquire()))
			{
				return;
			}

			if(FAILED(keyboard->GetDeviceState(256, keys)))
			{
				throw gcnew Exception("Input could not be aquired.");
			}
		}

		// We have valid date, we copy it to our array (arrays match).
		for(int i = 0; i < 256; i++)
		{
			button[i] = (keys[KeyMapper[i]] & 0x80) ? true : false;
		}

	}
	
	DIKeyboard::~DIKeyboard()
	{
		keyboard->Unacquire();
		keyboard->Release();
		delete [] keys;
	}

}
}
}
}