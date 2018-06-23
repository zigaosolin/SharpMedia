#include "RenderTargetView.h"
#include "Helper.h"

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	
	D3D10RenderTargetView::D3D10RenderTargetView(ID3D10RenderTargetView* view)
	{
		this->view = view;
	}

	D3D10RenderTargetView::~D3D10RenderTargetView()
	{
		this->view->Release();
	}

	void D3D10RenderTargetView::Clear(ID3D10Device* device, Colour colour)
	{
		float c[] = { colour.R, colour.G, colour.B, colour.A };
		device->ClearRenderTargetView(view, c);
	}

	

}
}
}
}