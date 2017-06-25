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
	outputColor.x = texture(pixels, vec2(uv.x, uv.y)).x;
	outputColor.y = texture(pixels, vec2(uv.x + 0.002f, uv.y)).y;
	outputColor.z = texture(pixels, vec2(uv.x, uv.y + 0.002f)).z;

	// retrieve brightness
	vec3 HDRBloom = max(outputColor - vec3(0.90f, 0.90f, 0.90f), vec3(0.0f, 0.0f, 0.0f));

	// retrieve distance
	float dx = P.x - 0.5, dy = P.y - 0.5;
	float distance = sqrt( dx * dx + dy * dy );

	// apply vignetting
	outputColor *= 1 / (distance + 0.25f) * 0.25f;
	
	// apply Gaussian blur
	float s = 0.84089642f, e = 2.71828f, pi = 3.14159f;			 // these values have been taken from wikipedia
	HDRBloom *= (1/ (2 * pi * s * s) * pow(e, (-dx * dx - dy * dy) / (2 * s * s)));
	// apply HDR Bloom
	outputColor += HDRBloom;
}

// EOF