cmake_minimum_required (VERSION 3.20)
project(magic_object_const)

add_custom_target(MagicConstRewriter
  WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/src
  COMMAND dotnet build MagicConstRewriter.csproj -c Release)
set(MAGIC_CONST_REWRITER_PATH ${CMAKE_BINARY_DIR}/bin/MagicConstRewriter/release/MagicConstRewriter)

add_subdirectory(example)
