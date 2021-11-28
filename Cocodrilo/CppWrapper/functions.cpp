#include "stdafx.h"
#include "functions.h"

#include <rhinoSdkCageObject.h>
#include <string>
#include <fstream>
#include <sstream>
#include <string>
#include <array>

typedef std::size_t SizeType;
typedef std::size_t IndexType;

int GetVectorFromMatrixIndices(const SizeType NumberPerRow, const SizeType NumberPerColumn, const SizeType NumberPerDepth,
    const IndexType RowIndex, const IndexType ColumnIndex, const IndexType DepthIndex) noexcept
{
    return DepthIndex * (NumberPerColumn * NumberPerRow) + ColumnIndex * NumberPerRow + RowIndex;
    
}

std::array<IndexType, 3> GetMatrixIndicesFromVectorIndex(
    const SizeType NumberPerRow,
    const SizeType NumberPerColumn,
    const SizeType NumberPerDepth,
    const IndexType Index) noexcept
{
    std::array<IndexType, 3> result;
    const IndexType index_in_row_column_plane = Index % (NumberPerRow * NumberPerColumn);
    result[0] = index_in_row_column_plane % NumberPerRow; // row
    result[1] = index_in_row_column_plane / NumberPerRow; // column 
    result[2] = Index / (NumberPerRow * NumberPerColumn);   // depth

    return result;
}


int CageEdit_cpp(GUID* guids, int n_guids, char* dirctory, int p_u, int p_v, int p_w, int cp_u, int cp_v, int cp_w, double scaling) {

    // Read result file
    std::vector<std::array<double, 3>> results{};
    std::ifstream infile(dirctory);
    //Todo check file!
    std::string line;
    bool start_to_read = false;
    while (std::getline(infile, line))
    {
        //RhinoApp().Print("Hallo\n");
        if (!(line.rfind("End") == 0) && start_to_read) {
            std::string values;
            std::stringstream ss(line);
            int id_tmp;
            std::array<double, 3>  values_{};
            int count = 0;
            while (std::getline(ss, values, ' ')) {
                if (values != " " && values != "") {
                    if (count == 0) {
                        id_tmp = std::stoi(values);
                    } else if (count < 4) {
                        values_[count - 1] = std::stod(values);
                    }
                    count++;
                }
            }
            results.push_back(values_);
        }
        if (line.rfind("Values") == 0) {
            start_to_read = true;
        }
    }

    //CRhinoCommand::result rc = CRhinoCommand::success;
    //CRhinoGetObject go;
    //go.SetCommandPrompt(L"Select captive surface or polysurface");
    //go.SetGeometryFilter(CRhinoGetObject::surface_object | CRhinoGetObject::polysrf_object);
    //go.GetObjects(1, 1);
    //rc = go.CommandResult();
    //if (CRhinoCommand::success != rc)
    //    return rc;

    //const CRhinoObject* captive_tt = go.Object(0).Object();
    //if (0 == captive_tt)
    //    return CRhinoCommand::failure;

    // Set up morph control
    ON_MorphControl* control = new ON_MorphControl();
    control->m_varient = 3; // 1= curve
    control->m_sporh_bPreserveStructure = false;
    
    ON_3dPoint* points = new ON_3dPoint[2];
    ON_BoundingBox bounding_box{ ON_3dPoint{ -3, -3, -0.0001 } , ON_3dPoint{ 3, 3, 5.0001 } };

    bool sucess = control->m_nurbs_cage.Create(bounding_box, p_u+1, p_v+1, p_w+1, cp_u, cp_v, cp_w);// = nurbs_cage;
    // Get Xform for transformation
    ON_Xform cage_xform{};
    ON_GetCageXform(control->m_nurbs_cage, cage_xform);
    control->m_nurbs_cage0 = cage_xform;

    CRhinoMorphControl* control_object = new CRhinoMorphControl(); //::Cast(control);
    control_object->SetControl(*control);

    RhinoApp().ActiveDoc()->AddObject(control_object);
    //// Set up the capture
    for (int i = 0; i < n_guids; ++i)
    {
        const CRhinoObject* captive = RhinoApp().ActiveDoc()->LookupObject(guids[i]);
        RhinoCaptureObject(control_object, const_cast<CRhinoObject*>(captive));
    }
    
    //RhinoCaptureObject(control_object, const_cast<CRhinoObject*>(captive_tt));
    //RhinoCaptureObject(control_object, const_cast<CRhinoObject*>(captive2));
 
    RhinoApp().ActiveDoc()->UnselectAll();

    control_object->EnableGrips(true);

    ON_SimpleArray< class CRhinoGripObject* > grip_list{};
    // Get Grip points
    control_object->GetGrips(grip_list);
    auto grip_itr_begin = grip_list.First();
    RhinoApp().Print( std::to_string( grip_list.Count() ).c_str() );
    RhinoApp().Print("\n");
    RhinoApp().Print(std::to_string(results.size()).c_str());
    RhinoApp().Print("\n");
    std::string a = std::to_string(grip_list.Count());
    RhinoApp().Print( a.c_str() );
    for (int i = 0; i < grip_list.Count(); ++i) {
        auto grip_itr = grip_itr_begin + i;
        
        std::array<IndexType,3> matrix_indices = GetMatrixIndicesFromVectorIndex(cp_w, cp_v, cp_u, i);
        int index = GetVectorFromMatrixIndices(cp_u, cp_v, cp_w, matrix_indices[2], matrix_indices[1], matrix_indices[0]);
        
        auto tmp_disp = results[index];
        ON_3dVector test{ scaling *tmp_disp[0], scaling *tmp_disp[1], scaling *tmp_disp[2] };
        if ((*grip_itr))
            (*grip_itr)->MoveGrip(test);
            

    }

	return 5;
}