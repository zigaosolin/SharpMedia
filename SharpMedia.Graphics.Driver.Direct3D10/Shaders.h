#pragma once
#include <windows.h>
#include <D3D10.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {


	public ref class D3D10PShader : public IPShader
	{
		ID3D10PixelShader* shader;
	public:
		void Apply(ID3D10Device* device);
		D3D10PShader(ID3D10PixelShader* shader);
		virtual ~D3D10PShader();
	};

	public ref class D3D10VShader : public IVShader
	{
		ID3D10VertexShader* shader;
	public:
		void Apply(ID3D10Device* device);
		D3D10VShader(ID3D10VertexShader* shader);
		virtual ~D3D10VShader();
	};

	public ref class D3D10GShader : public IGShader
	{
		ID3D10GeometryShader* shader;
	public:
		void Apply(ID3D10Device* device);
		D3D10GShader(ID3D10GeometryShader* shader);
		virtual ~D3D10GShader();
	};
	
}
}
}
}