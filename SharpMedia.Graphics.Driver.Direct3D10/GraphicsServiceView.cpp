#include <D3D10.h>
#include "GraphicsService.h"
#include "GraphicsServiceView.h"
#include "DeviceView.h"


namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	bool D3D10GraphicsServiceView::IsDeviceActive::get()
	{
		return service->Get() != 0;
	}

	D3D10GraphicsServiceView::D3D10GraphicsServiceView(D3D10GraphicsService^ service)
	{
		this->owner = false;
		this->service = service;
	}

	IDevice^ D3D10GraphicsServiceView::Create(bool shared, SharpMedia::Graphics::RenderTargetParameters ^parameters,
		SharpMedia::Graphics::Driver::ISwapChain ^%chain, SharpMedia::Graphics::Driver::IWindowBackend ^%window, bool debug)
	{
		bool tmp = owner;
		try {
			owner = true;
			return gcnew D3D10DeviceView(service->CreateDevice(shared, parameters, window, chain, debug), service);
		} catch(Exception^ ex)
		{
			owner = tmp;
			throw ex;
		}
	}

	IDevice^ D3D10GraphicsServiceView::Obtain()
	{
		if(service->Get() == 0) return nullptr;
		return gcnew D3D10DeviceView(service->Get(), service);
	}

	D3D10GraphicsServiceView::~D3D10GraphicsServiceView()
	{
		if(owner) 
		{
			service->Nullify();
			service = nullptr;
		}
	}

}
}
}
}