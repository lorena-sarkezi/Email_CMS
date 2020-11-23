import React, { useState, useEffect, useContext } from 'react';
import UserDropdown from './UserDropdown';
import {Link} from 'react-router-dom';

import axios from '../../axios';

import { AuthContext } from '../Auth/AuthContext';

import { Row, Col } from 'antd';

const Header = (props) => {

    const authContext = useContext(AuthContext);

    const [userUniqueName, setUserUniqueName] = useState(null);
    const [authState, setAuthState] = useState(false);
    const [isCallFinished, setIsCallFinished] = useState(false);

    const checkAuthStatus = async () => {
        //setIsCallFinished(false);
        const token = localStorage.getItem('token');
        if(token === null){
            setAuthState(false);
            setIsCallFinished(true);
        } 

        try{
            const request = await axios.get('/api/v1/identity/validate');

            setAuthState(request.data);
            setIsCallFinished(true);
            setUserUniqeNameFromToken();
        }
        catch(e){
            const response = e.response;

            setAuthState(false);
            setIsCallFinished(true);
        }
    }

    const setUserUniqeNameFromToken = () => {
        const claimsEncoded = localStorage.getItem("token").split(".")[1];
        const claimsStr = decodeURIComponent(atob(claimsEncoded).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        const claims = JSON.parse(claimsStr);

        setUserUniqueName(claims.unique_name);
    }

    useEffect(() => {
        console.log("Auth context");
        console.log(authContext);
        checkAuthStatus();
    },[isCallFinished, authContext])



    return (
        <Row>
            <Col flex="0 1 auto">
                <h1 className="header-text">
                    <Link to="/" style={{color:"white"}}>
                        EMAIL CMS
                    </Link>
                    
                </h1>
            </Col>
            <Col flex="auto"></Col>
            <Col flex="0 1 auto">
                {
                    isCallFinished && authState ?  <UserDropdown uniqueName={userUniqueName}/> : null
                }
            </Col>
        </Row>
    )
};

export default Header;