#version 330

// shader input
in vec2 P;						// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D pixels;		// input texture (1st pass render target)

// shader output
out vec3 outputColor;

void main()
{
	// retrieve input pixel
	outputColor = texture( pixels, uv ).rgb;
	// retrieve distance
	float dx = P.x - 0.5, dy = P.y - 0.5;
	float distance = sqrt( dx * dx + dy * dy );

	// apply vignetting and chromatic abberation
	outputColor.r *= 1 / (distance + 0.20f) * 0.25f;
	outputColor.g *= 1 / (distance + 0.15f) * 0.25f;
	outputColor.b *= 1 / (distance + 0.10f) * 0.25f;
}

// EOF