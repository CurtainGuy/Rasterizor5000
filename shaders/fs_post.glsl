#version 330

// shader input
in vec2 P;						// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
uniform sampler2D pixels;		// input texture (1st pass render target)

// shader output
out vec3 outputColor;

void main()
{
	float aberration = 1;
    // retrieve input pixel and apply chromatic aberration
    outputColor.x = texture(pixels, vec2(uv.x, uv.y)).x;
    outputColor.y = texture(pixels, vec2(uv.x + 0.002 * aberration, uv.y)).y;
    outputColor.z = texture(pixels, vec2(uv.x, uv.y + 0.002 * aberration)).z;

	// retrieve distance
	float dx = P.x - 0.5, dy = P.y - 0.5;
	float distance = sqrt( dx * dx + dy * dy );

	
	float vignetting = 1;
    // apply vignetting
    outputColor *= 1 / (vignetting *distance + 0.25f) * 0.25f;
}

// EOF