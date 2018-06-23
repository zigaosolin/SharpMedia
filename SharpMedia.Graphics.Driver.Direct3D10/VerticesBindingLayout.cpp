#include "VerticesBindingLayout.h"


namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {
	void D3D10VerticesBindingLayout::Apply(ID3D10Device* device)
	{
		device->IASetInputLayout(inputLayout);
	}


	D3D10VerticesBindingLayout::D3D10VerticesBindingLayout(ID3D10InputLayout* state)
	{
		this->inputLayout = state;
	}


	D3D10VerticesBindingLayout::~D3D10VerticesBindingLayout()
	{
		inputLayout->Release();
	}

}
}
}
}