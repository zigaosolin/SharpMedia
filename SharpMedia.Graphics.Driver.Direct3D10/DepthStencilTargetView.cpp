#include "DepthStencilTargetView.h"
#include "Helper.h"

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	
	D3D10DepthStencilTargetView::D3D10DepthStencilTargetView(ID3D10DepthStencilView* view)
	{
		this->view = view;
	}

	D3D10DepthStencilTargetView::~D3D10DepthStencilTargetView()
	{
		this->view->Release();
	}

	void D3D10DepthStencilTargetView::Clear(ID3D10Device* device, ClearOptions op, float depth, unsigned int stencil)
	{
		UINT clear = 0;
		if((int)op & (int)ClearOptions::Depth) clear |= D3D10_CLEAR_DEPTH;
		if((int)op & (int)ClearOptions::Stencil) clear |= D3D10_CLEAR_STENCIL;

		device->ClearDepthStencilView(view, clear, depth, stencil);
	}

	

}
}
}
}