#include "Shaders.h"


namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	void D3D10PShader::Apply(ID3D10Device* device)
	{
		device->PSSetShader(shader);
	}

	D3D10PShader::D3D10PShader(ID3D10PixelShader* shader)
	{
		this->shader = shader;
	}

	D3D10PShader::~D3D10PShader()
	{
		shader->Release();
	}

	void D3D10VShader::Apply(ID3D10Device* device)
	{
		device->VSSetShader(shader);
	}

	D3D10VShader::D3D10VShader(ID3D10VertexShader* shader)
	{
		this->shader = shader;
	}

	D3D10VShader::~D3D10VShader()
	{
		shader->Release();
	}

	void D3D10GShader::Apply(ID3D10Device* device)
	{
		device->GSSetShader(shader);
	}

	D3D10GShader::D3D10GShader(ID3D10GeometryShader* shader)
	{
		this->shader = shader;
	}

	D3D10GShader::~D3D10GShader()
	{
		shader->Release();
	}

}
}
}
}