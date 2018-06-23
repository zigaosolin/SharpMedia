#pragma once
#include <windows.h>
#include <D3D10.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {


	public ref class D3D10BlendState : public IBlendState
	{
		ID3D10BlendState* state;
	public:
		void Apply(ID3D10Device* device, Colour colour, unsigned int mask);
		D3D10BlendState(ID3D10BlendState* state);
		virtual ~D3D10BlendState();
	};

	public ref class D3D10RasterizationState : public IRasterizationState
	{
		ID3D10RasterizerState* state;
	public:
		void Apply(ID3D10Device* device);
		D3D10RasterizationState(ID3D10RasterizerState* state);
		virtual ~D3D10RasterizationState();
	};

	public ref class D3D10DepthStencilState : public IDepthStencilState
	{
		ID3D10DepthStencilState* state;
	public:
		void Apply(ID3D10Device* device, unsigned int reference);
		D3D10DepthStencilState(ID3D10DepthStencilState* state);
		virtual ~D3D10DepthStencilState();
	};

	public ref class D3D10SamplerState : public ISamplerState
	{
	public:
		ID3D10SamplerState* state;
		D3D10SamplerState(ID3D10SamplerState* state);
		virtual ~D3D10SamplerState();
	};

}
}
}
}