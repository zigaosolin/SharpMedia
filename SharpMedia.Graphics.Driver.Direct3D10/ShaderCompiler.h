#pragma once
#include <windows.h>
#include <D3D10.h>
#include <string>
#include <map>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	struct D3D10CompilationData
	{
		std::string code;
		std::string inputs;
		std::string outputs;
		std::string uniforms[Shaders::ConstantBufferLayout::MaxConstantBufferBindingSlots];
		std::string texturesAndSamplers;
		int outCounter;
	};


	// A HLSL shader compiler. It can run independantly of Device. 
	public ref class D3D10ShaderCompiler : public IShaderCompiler
	{
		BindingStage shaderType;
		ID3D10Device* device;
		D3D10CompilationData* data; //< A special "struct" that holds unmanaged data.

		void BinaryOp(int n1, int n2, int dst, std::string op);
	public:
		// Ends the shader compilation, resulting in a blob that contains bytecode.
		ID3D10Blob* EndBytecode();


		D3D10ShaderCompiler(ID3D10Device* device);
		virtual ~D3D10ShaderCompiler();
		virtual IShaderBase^ Compile(BindingStage t, String^ code);
        virtual void Begin(BindingStage t);
        virtual IShaderBase^ End();
		virtual void Convert(int n, PinFormat outFormat, int result);
        virtual void RegisterInput(int n, PinFormat fmt, PinComponent component);
        virtual void RegisterConstant(int n, PinFormat fmt, 
					unsigned int arraySize, unsigned int buffer, unsigned int position);
        virtual void RegisterFixed(int n, PinFormat fmt, unsigned int arraySize, Object^ data);
        virtual void RegisterTemp(int n, PinFormat fmt, unsigned int arraySize);
        virtual void Add(int n1, int n2, int name);
        virtual void Sub(int n1, int n2, int name);
        virtual void Div(int n1, int n2, int name);
        virtual void Mul(int n1, int n2, int name);
		virtual void MulEx(int n1, int n2, int name);
        virtual void Dot(int n1, int n2, int name);
		virtual void Swizzle(int n, SwizzleMask^ mask, int name);
        virtual void Mov(int n, int o);
        virtual void BeginIf(int n);
        virtual void Else();
        virtual void EndIf();
        virtual void BeginWhile();
        virtual void EndWhile();
		virtual void Break(int n);
        virtual void BeginSwitch(int n);
        virtual void BeginCase(int n);
        virtual void BeginDefault();
        virtual void EndCase();
        virtual void EndSwitch();
        virtual void IndexInArray(int arr, int index, int outName);
		virtual void Expand(int n1, int n2, PinFormat from, PinFormat to, Shaders::ExpandType type);
        virtual void Output(PinComponent component, PinFormat fmt, int n);
		virtual void Call(Shaders::ShaderFunction function, int n1, int n2);
		virtual void Compare(States::CompareFunction function, int n1, int n2, int r);
		virtual void Sample(int sampler, int texture, int pos, int offset, int result);
		virtual void Load(int texture, int pos, int offset, int result);
		virtual void RegisterSampler(int n, unsigned int reg);
		virtual void RegisterTexture(int n, PinFormat fmt, PinFormat textureFmt, UInt32 reg);
		virtual void Min(int n1, int n2, int dst);
		virtual void Max(int n1, int n2, int dst);
	};


}
}
}
}
