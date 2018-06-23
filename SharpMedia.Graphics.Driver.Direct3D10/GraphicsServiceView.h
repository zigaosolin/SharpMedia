#pragma once

#include "GraphicsService.h"

using namespace System;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	public ref class D3D10GraphicsServiceView : public IGraphicsService
	{
	protected:
		bool owner;
		D3D10GraphicsService^ service;
	public:
		D3D10GraphicsServiceView(D3D10GraphicsService^ service);
		virtual property bool IsDeviceActive
		{
			bool get();
		}
		virtual IDevice^ Create(bool shared, RenderTargetParameters^ parameters,
                        ISwapChain^% chain, IWindowBackend^% window, bool debug);
        virtual IDevice^ Obtain();
		virtual ~D3D10GraphicsServiceView();
	};

}
}
}
}