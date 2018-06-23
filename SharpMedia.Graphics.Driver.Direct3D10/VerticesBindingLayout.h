#pragma once
#include <windows.h>
#include <D3D10.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {


	public ref class D3D10VerticesBindingLayout : public IVerticesBindingLayout
	{
		ID3D10InputLayout* inputLayout;
	public:
		void Apply(ID3D10Device* device);
		D3D10VerticesBindingLayout(ID3D10InputLayout* state);
		virtual ~D3D10VerticesBindingLayout();
	};


}
}
}
}
