#pragma once
#include <windows.h>

using namespace System;
using namespace SharpMedia::ComponentOS::Applications;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	public ref class Application : public CommandLineApplication {
		public override int Run(String args[]) {
			this.Console.WriteLine("Hello, I am the Direct3D 10 Process... I should start my engines now");
		}
	}
}
}
}
}