#include "States.h"


namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	D3D10BlendState::D3D10BlendState(ID3D10BlendState* state)
	{
		this->state = state;
	}


	void D3D10BlendState::Apply(ID3D10Device* device, Colour colour, unsigned int mask)
	{
		FLOAT factor[4];
		factor[0] = colour.R;
		factor[1] = colour.G;
		factor[2] = colour.B;
		factor[3] = colour.A;
		device->OMSetBlendState(state, factor, mask);
	}

	D3D10BlendState::~D3D10BlendState()
	{
		state->Release();
	}


	D3D10RasterizationState::D3D10RasterizationState(ID3D10RasterizerState* state)
	{
		this->state = state;
	}


	void D3D10RasterizationState::Apply(ID3D10Device* device)
	{
		device->RSSetState(state);
	}

	D3D10RasterizationState::~D3D10RasterizationState()
	{
		state->Release();
	}

	D3D10DepthStencilState::D3D10DepthStencilState(ID3D10DepthStencilState* state)
	{
		this->state = state;
	}


	void D3D10DepthStencilState::Apply(ID3D10Device* device, unsigned int reference)
	{
		device->OMSetDepthStencilState(state, reference);
	}

	D3D10DepthStencilState::~D3D10DepthStencilState()
	{
		state->Release();
	}

	D3D10SamplerState::D3D10SamplerState(ID3D10SamplerState* state)
	{
		this->state = state;
	}

	D3D10SamplerState::~D3D10SamplerState()
	{
		state->Release();
	}

}
}
}
}