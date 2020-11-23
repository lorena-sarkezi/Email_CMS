import React, {useContext} from 'react';
import {useHistory} from 'react-router-dom';

import { Dropdown, Menu, Button, Avatar } from 'antd';

import {AuthContext} from '../Auth/AuthContext';

import UserOutlined from '@ant-design/icons/UserOutlined';
import DownOutlined from '@ant-design/icons/DownOutlined';

const UserDropdown = props => {
    const authContext = useContext(AuthContext);
    
    const history = useHistory();

    const handleMenuClick = (e) => {
        if(e.key == 1){
            localStorage.removeItem("token");
            authContext.value=false;
            window.location.reload();
        }
        
    }

    const DropdownMenu = () => {
        
        return(
            <Menu onClick={handleMenuClick}>
                <Menu.Item key={1}>
                    Logout
                </Menu.Item>
            </Menu>
        )
    }

    const DropdownStyle={
        "background-color": "#001529",
        "color": "white",
        "border": "none"
    };

    return(
        <Dropdown overlay={DropdownMenu} trigger={["click"]}>
            <Button style={DropdownStyle}>
                <Avatar icon={<UserOutlined/>} style={{marginRight:"10px"}}/> {props.uniqueName} <DownOutlined />
            </Button>
        </Dropdown>   
    )
} 


export default UserDropdown;