#ifndef FUNCTIONS_H_INCLUDE
#define FUNCTIONS_H_INCLUDE

#define CPPWrapper_C_FUNCTION extern "C" __declspec(dllexport)

CPPWrapper_C_FUNCTION
int CageEdit_cpp(GUID* guids, int n_guids, char* dirctory, int p_u, int p_v, int p_w, int cp_u, int cp_v, int cp_w, double scaling);

#endif