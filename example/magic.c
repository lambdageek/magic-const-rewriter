
#include "magic.h"

uint8_t magic_constant[] = {
	0xAA, 0xBB, 0x33, 0x55, // magic prefix
	0x2E, 0x4E, 0x45, 0x54, // ".NET"
	0x00, 0x00, 0x00, 0x00, // nul terminate
};
