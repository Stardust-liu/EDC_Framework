using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomizeUI{
    public class CustomizeBtn : Button
    {
        [SerializeField]
        private int m_test = 0;
        public int Test{
            get{
                return m_test;
            }
            set{
                m_test = value;
                ChangeTest(m_test);
            }
        }

        [SerializeField]
        private bool m_isOpen = false;

        public bool IsOpen{
            get{
                return m_isOpen;
            }
            set{
                m_isOpen = value;
                ChangeIsOpen(m_isOpen);
            }
        }
        
        public void ChangeIsOpen(bool value){

        }

        public void ChangeTest(int value){
            Debug.Log(value);
        }
    }
}
