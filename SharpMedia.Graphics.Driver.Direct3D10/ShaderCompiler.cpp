#include "ShaderCompiler.h"
#include "Helper.h"
#include "Shaders.h"
#include <d3dx10.h>

using namespace System::Text;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	D3D10ShaderCompiler::D3D10ShaderCompiler(ID3D10Device* device)
	{
		this->device = device;
		this->data = new D3D10CompilationData;
		device->AddRef();
	}

	D3D10ShaderCompiler::~D3D10ShaderCompiler()
	{
		// Make sure we release reference.
		device->Release();
		delete this->data;
	}

	IShaderBase^ D3D10ShaderCompiler::Compile(BindingStage t, String^ filename)
	{
		// Choose a profile.
		const char* profile = 0;
		switch(t)
		{
		case BindingStage::GeometryShader:
			profile = D3D10GetGeometryShaderProfile(device);
			break;
		case BindingStage::PixelShader:
			profile = D3D10GetPixelShaderProfile(device);
			break;
		case BindingStage::VertexShader:
			profile = D3D10GetVertexShaderProfile(device);
			break;
		}

		// We copy the filename.
		Char* buffer = new Char[filename->Length+1];

		Encoding^ ascii = Encoding::ASCII;
		array<Byte>^ arr = ascii->GetBytes(filename);
		
		int i;
		for(i = 0; i < filename->Length; i++)
		{
			buffer[i] = (CHAR)filename[i];
		}
		buffer[i] = 0;



		ID3D10Blob* bytecode = 0, *errors = 0;
		try {

			// We have complete shader, we compile it now.
			if(FAILED(D3DX10CompileFromFile(buffer, 
				0, 0, "main", profile, 0, D3D10_SHADER_DEBUG, 0, &bytecode, &errors, 0)))
			{
				String^ errorString = gcnew String((char*)errors->GetBufferPointer());
				throw gcnew Exception(errorString);
			}

		} finally {
			if(errors != 0) errors->Release();
		}
		

		// We construct shader.
		try {

			// We have valid shader, all we need is to compile it.
			switch(t)
			{
			case BindingStage::PixelShader:
				{
					ID3D10PixelShader* shader;
					DXFAILED(device->CreatePixelShader(bytecode->GetBufferPointer(), 
						bytecode->GetBufferSize(), &shader));

					// We have a valid shader, return it.
					return gcnew D3D10PShader(shader);
				}
			case BindingStage::VertexShader:
				{
					ID3D10VertexShader* shader;
					DXFAILED(device->CreateVertexShader(bytecode->GetBufferPointer(), 
						bytecode->GetBufferSize(), &shader));

					// We have a valid shader, return it.
					return gcnew D3D10VShader(shader);
				}
			case BindingStage::GeometryShader:
				{
					ID3D10GeometryShader* shader;
					DXFAILED(device->CreateGeometryShader(bytecode->GetBufferPointer(), 
						bytecode->GetBufferSize(), &shader));

					// We have a valid shader, return it.
					return gcnew D3D10GShader(shader);
				}
			default:
				NOT_SUPPORTED();
			}

		} finally
		{
			if(bytecode != 0) bytecode->Release();
		}
	}

	void D3D10ShaderCompiler::Begin(BindingStage t)
	{
		shaderType = t;

		// We clear all text.
		data->code.clear();
		data->inputs.clear();
		data->outputs.clear();
		data->texturesAndSamplers.clear();
		for(unsigned int i = 0; i < Shaders::ConstantBufferLayout::MaxConstantBufferBindingSlots; i++)
		{
			data->uniforms[i].clear();
		}

		// We try to guess capacity.
		data->code.reserve(2048);
		data->inputs.reserve(256);
		data->outputs.reserve(256);	
		data->outCounter = 0;
	}

	void D3D10ShaderCompiler::Sample(int sampler, int texture, int pos, int off, int result)
	{
		std::string c("_");
		c.append(ConvToString(result));
		c.append("=_");
		c.append(ConvToString(texture));
		c.append(".Sample(_");
		c.append(ConvToString(sampler));
		c.append(",_");
		c.append(ConvToString(pos));
		if(off != -1)
		{
		  c.append(",_");
		  c.append(ConvToString(off));
		}
		c.append(");");

		data->code.append(c);
	}

	void D3D10ShaderCompiler::Load(int texture, int pos, int offset, int result)
	{
		std::string c("_");
		c.append(ConvToString(result));
		c.append("=_");
		c.append(ConvToString(texture));
		c.append(".Load(_");
		c.append(ConvToString(pos));
		if(offset != -1)
		{
			c.append(",_");
			c.append(ConvToString(offset));
		}
		c.append(");");

		data->code.append(c);
	}

	void D3D10ShaderCompiler::Min(int n1, int n2, int dst)
	{
		std::string c("_");
		c.append(ConvToString(dst));
		c.append("=min(_");
		c.append(ConvToString(n1));
		c.append(",_");
		c.append(ConvToString(n2));
		c.append(");");

		data->code.append(c);
	}

	void D3D10ShaderCompiler::Max(int n1, int n2, int dst)
	{
		std::string c("_");
		c.append(ConvToString(dst));
		c.append("=max(_");
		c.append(ConvToString(n1));
		c.append(",_");
		c.append(ConvToString(n2));
		c.append(");");

		data->code.append(c);
	}

	void D3D10ShaderCompiler::RegisterSampler(int n, unsigned int reg)
	{
		std::string c("sampler _");
		c.append(ConvToString(n));
		c.append(": register(s");
		c.append(ConvToString(reg));
		c.append(");");
		
		data->texturesAndSamplers.append(c);
	}

	void D3D10ShaderCompiler::RegisterTexture(int n, PinFormat fmt, PinFormat textureFmt, UInt32 reg)
	{
		std::string type;
		switch(fmt)
		{
		case PinFormat::Texture1D:
			type = "Texture1D";
			break;
		case PinFormat::Texture1DArray:
			type = "Texture1DArray";
			break;
		case PinFormat::Texture2D:
			type = "Texture2D";
			break;
		case PinFormat::Texture2DArray:
			type = "Texture2DArray";
			break;
		case PinFormat::TextureCube:
			type = "TextureCube";
			break;
		case PinFormat::Texture3D:
			type = "Texture3D";
			break;
		case PinFormat::BufferTexture:
			type = "Buffer";
			break;
		default:
			NOT_SUPPORTED();

		}

		std::string textureType = ConvToString(textureFmt);

		std::string c(type);
		c.append("<");
		c.append(textureType);
		c.append(">");
		c.append(" _");
		c.append(ConvToString(n));
		c.append(": register(t");
		c.append(ConvToString(reg));
		c.append(");");


		data->texturesAndSamplers.append(c);
	}

	ID3D10Blob* D3D10ShaderCompiler::EndBytecode()
	{
		// Choose a profile.
		const char* profile = 0;
		switch(shaderType)
		{
		case BindingStage::GeometryShader:
			profile = D3D10GetGeometryShaderProfile(device);
			break;
		case BindingStage::PixelShader:
			profile = D3D10GetPixelShaderProfile(device);
			break;
		case BindingStage::VertexShader:
			profile = D3D10GetVertexShaderProfile(device);
			break;
		}

		// We create a code buffer and make sure we reserve buffer big enough.
		std::string hlsl;
		hlsl.reserve(data->code.size()+data->inputs.size()+data->outputs.size()+512);

		// We start writing a shader.
		
		// Create constant buffers.
		for(int i = 0; i < Shaders::ConstantBufferLayout::MaxConstantBufferBindingSlots; ++i)
		{
			std::string &constantBuffer = data->uniforms[i];

			// Skip unused.
			if(constantBuffer.empty()) continue;

			// Create buffer description.
			hlsl.append("cbuffer _Buffer_");
			hlsl.append(ConvToString(i));
			hlsl.append(" : register(b");
			hlsl.append(ConvToString(i));
			hlsl.append("){\n");
			hlsl.append(constantBuffer);
			hlsl.append("}\n");
		}

		hlsl.append(data->texturesAndSamplers);


		hlsl.append("void main(");
		hlsl.append(data->inputs);
		hlsl.append(data->outputs);

		// Make sure we overwrite last ',' with ')'.
		hlsl.replace(hlsl.size()-1, 1, ")");
		hlsl.append("{\n");
		hlsl.append(data->code);
		hlsl.append("}\n");


		ID3D10Blob* bytecode = 0, *errors = 0;
		try {

		// We have complete shader, we compile it now.
		if(FAILED(D3D10CompileShader(hlsl.c_str(), hlsl.size(), "", 
			0, 0, "main", profile, D3D10_SHADER_PACK_MATRIX_ROW_MAJOR, &bytecode, &errors)))
		{
			Console::WriteLine(gcnew String(hlsl.c_str()));
			Common::Error(D3D10ShaderCompiler::typeid, gcnew String(hlsl.c_str()));

			String^ errorString = gcnew String((char*)errors->GetBufferPointer());
			throw gcnew Exception(errorString);
		} else {
			Console::WriteLine(gcnew String(hlsl.c_str()));
			Common::Error(D3D10ShaderCompiler::typeid, gcnew String(hlsl.c_str()));
		}

		} finally {
			if(errors != 0) errors->Release();
		}
		return bytecode;
	}

	IShaderBase^ D3D10ShaderCompiler::End()
	{

		ID3D10Blob* bytecode = 0;
		try {
			// Obtain bytecode first.
			bytecode = EndBytecode();

			// We have valid shader, all we need is to compile it.
			switch(shaderType)
			{
			case BindingStage::PixelShader:
				{
					ID3D10PixelShader* shader;
					DXFAILED(device->CreatePixelShader(bytecode->GetBufferPointer(), 
						bytecode->GetBufferSize(), &shader));

					// We have a valid shader, return it.
					return gcnew D3D10PShader(shader);
				}
			case BindingStage::VertexShader:
				{
					ID3D10VertexShader* shader;
					DXFAILED(device->CreateVertexShader(bytecode->GetBufferPointer(), 
						bytecode->GetBufferSize(), &shader));

					// We have a valid shader, return it.
					return gcnew D3D10VShader(shader);
				}
			case BindingStage::GeometryShader:
				{
					ID3D10GeometryShader* shader;
					DXFAILED(device->CreateGeometryShader(bytecode->GetBufferPointer(), 
						bytecode->GetBufferSize(), &shader));

					// We have a valid shader, return it.
					return gcnew D3D10GShader(shader);
				}
			default:
				NOT_SUPPORTED();
			}

		} finally
		{
			if(bytecode != 0) bytecode->Release();
		}

	}


	void D3D10ShaderCompiler::Call(Shaders::ShaderFunction function, int n1, int n2)
	{
		std::string c("_");
		c.append(ConvToString(n2));
		c.append("=");

		switch(function)
		{
		case Shaders::ShaderFunction::Floor:
			c.append("floor(_");
			break;
		case Shaders::ShaderFunction::Abs:
			c.append("abs(_");
			break;
		case Shaders::ShaderFunction::Length:
			c.append("length(_");
			break;
		case Shaders::ShaderFunction::All:
			c.append("all(_");
			break;
		case Shaders::ShaderFunction::Any:
			c.append("any(_");
			break;
		case Shaders::ShaderFunction::Ceil:
			c.append("ceil(_");
			break;
		case Shaders::ShaderFunction::None:
			c.append("!all(_");
			break;
		default:
			NOT_SUPPORTED();
		}
		c.append(ConvToString(n1));
		c.append(");");

		data->code.append(c);
	}

	void D3D10ShaderCompiler::Convert(int n, PinFormat outFormat, int result)
	{
		std::string c("_");
		c.append(ConvToString(result));
		c.append("=(");
		c.append(ConvToString(outFormat));
		c.append(")_");
		c.append(ConvToString(n));
		c.append(";");

		data->code.append(c);
	}


	void D3D10ShaderCompiler::Compare(States::CompareFunction function, int n1, int n2, int r)
	{
		std::string c("_");
		c.append(ConvToString(r));
		c.append("=_");
		c.append(ConvToString(n1));
		
		switch(function)
		{
		case States::CompareFunction::LessEqual:
			c.append("<=_");
			break;
		case States::CompareFunction::Less:
			c.append("<_");
			break;
		case States::CompareFunction::Greater:
			c.append(">_");
			break;
		case States::CompareFunction::GreaterEqual:
			c.append(">=_");
			break;
		case States::CompareFunction::Equal:
			c.append("==_");
			break;
		case States::CompareFunction::NotEqual:
			c.append("!=_");
			break;
		default:
			NOT_SUPPORTED();
		}
		c.append(ConvToString(n2));
		c.append(";");

		data->code.append(c);

	}

	void D3D10ShaderCompiler::RegisterInput(int n, PinFormat fmt, PinComponent component)
	{
		std::string input("in ");
		input.append(ToDXString(fmt));
		input.append(" _");
		input.append(ConvToString(n));
		input.append(":");
		input.append(ToDXString(component));
		input.append(",");

		// Add to inputs.
		data->inputs.append(input);
	}

	void D3D10ShaderCompiler::RegisterConstant(int n, PinFormat fmt, unsigned int arraySize,
		unsigned int buffer, unsigned int position)
	{
		std::string uniform(ToDXString(fmt));
		uniform.append(" _");
		uniform.append(ConvToString(n));
		if(arraySize != UInt32::MaxValue)
		{
			uniform.append("[");
			uniform.append(ConvToString((int)arraySize));
			uniform.append("]");
		}

		// We add packing.
		uniform.append(":packoffset(c");
		uniform.append(ConvToString((int)(position/16)));
		switch((position/4)%4)
		{
		case 0: uniform.append(".x);\n"); break;
		case 1: uniform.append(".y);\n"); break;
		case 2: uniform.append(".z);\n"); break;
		case 3: uniform.append(".w);\n"); break;
		}
		

		// Add to correct uniforms.
		data->uniforms[buffer].append(uniform);
	}

	void D3D10ShaderCompiler::RegisterFixed(int n, PinFormat fmt, unsigned int arraySize, Object^ _data)
	{
		std::string c("const ");
		c.append(ToDXString(fmt));
		c.append(" _");
		c.append(ConvToString(n));
		if(arraySize != UInt32::MaxValue)
		{
			c.append("[");
			c.append(ConvToString((int)arraySize));
			c.append("]");
		}
		c.append("=");
		c.append(ToDXValue(fmt, _data));
		c.append(";\n");

		// Add to code (inline).
		data->code.append(c);
	}

	void D3D10ShaderCompiler::RegisterTemp(int n, PinFormat fmt, unsigned int arraySize)
	{
		std::string c(ToDXString(fmt));
		c.append(" _");
		c.append(ConvToString(n));
		if(arraySize != UInt32::MaxValue)
		{
			c.append("[");
			c.append(ConvToString((int)arraySize));
			c.append("]");
		}
		c.append(";\n");

		// Add temporary to code.
		data->code.append(c);
	}

	void D3D10ShaderCompiler::BinaryOp(int n1, int n2, int dst, std::string op)
	{
		std::string c("_");
		c.append(ConvToString(dst));
		c.append("=_");
		c.append(ConvToString(n1));
		c.append(op);
		c.append("_");
		c.append(ConvToString(n2));
		c.append(";");

		// Add instruction to code.
		data->code.append(c);
	}

	void D3D10ShaderCompiler::Add(int n1, int n2, int name)
	{
		BinaryOp(n1, n2, name, "+");
	}

	void D3D10ShaderCompiler::Sub(int n1, int n2, int name)
	{
		BinaryOp(n1, n2, name, "-");
	}

	void D3D10ShaderCompiler::Div(int n1, int n2, int name)
	{
		BinaryOp(n1, n2, name, "/");
	}

	void D3D10ShaderCompiler::Mul(int n1, int n2, int name)
	{
		BinaryOp(n1, n2, name, "*");
	}

	void D3D10ShaderCompiler::MulEx(int n1, int n2, int name)
	{
		std::string c("_");
		c.append(ConvToString(name));
		c.append("=mul(_");
		c.append(ConvToString(n1));
		c.append(",_");
		c.append(ConvToString(n2));
		c.append(");");

		// Add instruction to code.
		data->code.append(c);
	}

	void D3D10ShaderCompiler::Dot(int n1, int n2, int name)
	{
		std::string c("_");
		c.append(ConvToString(name));
		c.append("=dot(_");
		c.append(ConvToString(n1));
		c.append(",_");
		c.append(ConvToString(n2));
		c.append(");");

		// Add instruction to code.
		data->code.append(c);
	}

	void D3D10ShaderCompiler::Swizzle(int n, SwizzleMask^ mask, int name)
	{
		std::string c("_");
		c.append(ConvToString(name));
		c.append("=_");
		c.append(ConvToString(n));
		c.append(".");
		c.append(ToDXSwizzle(mask));
		c.append(";");

		// Add to code.
		data->code.append(c);
	}

	void D3D10ShaderCompiler::BeginIf(int n)
	{
		std::string c("if(_");
		c.append(ConvToString(n));
		c.append(") {");

		// Add to code.
		data->code.append(c);
	}

    void D3D10ShaderCompiler::Else()
	{
		// Add to code.
		data->code.append(" } else { ");
	}

    void D3D10ShaderCompiler::EndIf()
	{
		data->code.append("}");
	}

    void D3D10ShaderCompiler::BeginWhile()
	{
		std::string c("while(1) {");

		data->code.append(c);
	}

	void D3D10ShaderCompiler::Break(int n)
	{
		std::string c("if(!_");
		c.append(ConvToString(n));
		c.append(") break;");

		data->code.append(c);
	}

    void D3D10ShaderCompiler::EndWhile()
	{
		data->code.append("}");
	}

    void D3D10ShaderCompiler::BeginSwitch(int n)
	{
		std::string c("switch(_");
		c.append(ConvToString(n));
		c.append(") {");


		data->code.append(c);
	}

    void D3D10ShaderCompiler::BeginCase(int n)
	{
		std::string c("case _");
		c.append(ConvToString(n));
		c.append(": {");


		data->code.append(c);
	}

    void D3D10ShaderCompiler::BeginDefault()
	{
		data->code.append("default: {");
	}

    void D3D10ShaderCompiler::EndCase()
	{
		std::string c("} break;");


		data->code.append(c);
	}

    void D3D10ShaderCompiler::EndSwitch()
	{
		data->code.append("}");
	}

    void D3D10ShaderCompiler::IndexInArray(int arr, int index, int outName)
	{
		std::string c("_");
		c.append(ConvToString(outName));
		c.append("=_");
		c.append(ConvToString(arr));
		c.append("[_");
		c.append(ConvToString(index));
		c.append("];");

		data->code.append(c);
	}

	std::string ExpandTypeForPos(int pos, Shaders::ExpandType type)
	{
		switch(type)
		{
		case Shaders::ExpandType::AddOnes:
			return "1";
		case Shaders::ExpandType::AddOnesAtW:
			if(pos == 3) return "1";
			return "0";
		case Shaders::ExpandType::AddZeros:
			return "0";
		default:
			NOT_SUPPORTED();
		}
	}

	void D3D10ShaderCompiler::Expand(int n1, int n2, PinFormat from, PinFormat to, Shaders::ExpandType type)
	{
		std::string c("_");
		c.append(ConvToString(n2));
		c.append("=");

		// Expansion type calculation.
		UInt32 size1, size2;
		PinFormat scalar;
		if(!PinFormatHelper::IsVector(to, scalar, size1) ||
			!PinFormatHelper::IsVector(from, scalar, size2))
		{
			NOT_SUPPORTED();
		}

		// We now do the expansion.
		c.append(ConvToString(to));
		c.append("(_");
		c.append(ConvToString(n1));

		// We now expand it.
		while(size2 < size1)
		{
			c.append(",");
			c.append(ExpandTypeForPos(size2++, type));
		}
		c.append(");");

		data->code.append(c);

	}


	void D3D10ShaderCompiler::Mov(int n, int o)
	{
		std::string c("_");
		c.append(ConvToString(o));
		c.append("=_");
		c.append(ConvToString(n));
		c.append(";");

		// Add to code.
		data->code.append(c);
	}

	void D3D10ShaderCompiler::Output(PinComponent component, PinFormat fmt, int n)
	{
		// Copy part.
		std::string c("__");
		c.append(ConvToString(data->outCounter));
		c.append("=_");
		c.append(ConvToString(n));
		c.append(";");

		// Definition part.
		std::string output("out ");
		output.append(ToDXString(fmt));
		output.append(" __");
		output.append(ConvToString(data->outCounter));
		output.append(":");
		output.append(ToDXString(component));
		output.append(",");

		// Add to inputs.
		data->outputs.append(output);

		++data->outCounter;

		// Add to code.
		data->code.append(c);
	}

}}}}