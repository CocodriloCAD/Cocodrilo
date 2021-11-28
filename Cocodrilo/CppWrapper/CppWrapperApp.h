// CppWrapper.h : main header file for the CppWrapper DLL.
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols

// CCppWrapperApp
// See CppWrapperApp.cpp for the implementation of this class
//

class CCppWrapperApp : public CWinApp
{
public:
	CCppWrapperApp() = default;

// Overrides
public:
	BOOL InitInstance() override;
	int ExitInstance() override;
	DECLARE_MESSAGE_MAP()
};

// CSplashWnd
// See CppWrapperApp.cpp for the implementation of this class
//

class CSplashWnd : public CWnd
{
	DECLARE_DYNAMIC(CSplashWnd)

public:
	CSplashWnd();
	virtual ~CSplashWnd();

protected:
  CBitmap m_splash_bitmap;

protected:
	DECLARE_MESSAGE_MAP()

public:
  afx_msg void OnPaint();
  afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
};
