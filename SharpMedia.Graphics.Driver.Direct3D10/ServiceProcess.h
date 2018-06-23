#pragma once

using namespace System;
using namespace SharpMedia;
using namespace SharpMedia::Components;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {


	/// A service process, running behind the scenes.
	public ref class D3D10ServiceProcess : Applications::Application
	{
		Services::IServiceRegistry^ serviceRegistry;
	public:
		D3D10ServiceProcess();

		[SharpMedia::Components::Configuration::RequiredAttribute]
		virtual property Services::IServiceRegistry^ ServiceRegistry
        {
			Services::IServiceRegistry^ get();
			void set(Services::IServiceRegistry^ value);
        }

        virtual int Start(array<String^>^ args) override;
	};

}
}
}
}