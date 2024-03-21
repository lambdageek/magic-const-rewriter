cmake_minimum_required (VERSION 3.20)
project (MagicConstExample C)

add_library(MagicObject OBJECT magic.c)

add_custom_command(OUTPUT ${CMAKE_CURRENT_BINARY_DIR}/modified_magic.o
  COMMAND ${MAGIC_CONST_REWRITER_PATH} $<TARGET_OBJECTS:MagicObject> -o ${CMAKE_CURRENT_BINARY_DIR}/modified_magic.o
  DEPENDS MagicConstRewriter $<TARGET_OBJECTS:MagicObject> 
)


add_library(ModifiedMagic IMPORTED OBJECT)
set_target_properties (ModifiedMagic PROPERTIES IMPORTED_OBJECTS ${CMAKE_CURRENT_BINARY_DIR}/modified_magic.o)

add_executable(example main.c $<TARGET_OBJECTS:ModifiedMagic>)
