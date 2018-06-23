
#include "GraphicsService.h"
#include "GraphicsServiceView.h"
#include "ServiceProcess.h"


namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	D3D10ServiceProcess::D3D10ServiceProcess()
	{
		serviceRegistry = nullptr;
	}

	Services::IServiceRegistry^ D3D10ServiceProcess::ServiceRegistry::get()
	{
		return serviceRegistry;
	}

	void D3D10ServiceProcess::ServiceRegistry::set(Services::IServiceRegistry^ value)
	{
		serviceRegistry = value;
	}

	int D3D10ServiceProcess::Start(array<String^>^ args)
	{
		// We first create service singleton
		Components::Configuration::ComponentProviders::Instance^ singleton = 
			gcnew Components::Configuration::ComponentProviders::Instance(gcnew D3D10GraphicsService());

		// The provider.
		Components::Configuration::ComponentProviders::InstanceViewSupport^ provider = 
			gcnew Components::Configuration::ComponentProviders::InstanceViewSupport(singleton, D3D10GraphicsServiceView::typeid, IGraphicsService::typeid);

		serviceRegistry->RegisterServiceComponent(provider);

		// Sleep forever.
		System::Threading::Thread::Sleep(int::MaxValue);

		return 0;
	}

}
}
}
}