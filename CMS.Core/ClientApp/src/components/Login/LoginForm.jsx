import React, { useState, useEffect, useContext } from 'react';
import { Link, useHistory } from 'react-router-dom';
import axios from '../../axios';
import { Form, Input, Button, Checkbox, Alert, Spin } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';

import {AuthContext} from '../Auth/AuthContext';

const initialFormState = {
    username: "",
    password: "",
    remember: false
};

const AlertMesage = ({ message }) =>
    <Alert
        showIcon
        message={message}
        type="error"
        className="login-alert"
        closable={false}

    />


const LoginForm = props => {

    const authContext = useContext(AuthContext);

    const history = useHistory();

    const msgLogin = "Incorrect Username or Password!";
    const msgGenericError = "An error occured";

    const [formState, setFormState] = useState(initialFormState);
    const [credentialsIncorrect, setCredentialsIncorrect] = useState(false);
    const [isGenericError, setIsGenericError] = useState(false);
    const [isLoading, setIsLoading] = useState(false);

    const handleChangeFormState = (value, prop) => {
        let data = { ...formState };
        data[prop] = value;
        setFormState(data);
    }

    const handleSubmit = async (e) => {
        //e.preventDefault();  //Prevent form submit

        setCredentialsIncorrect(false);
        setIsGenericError(false);

        let data = { ...formState };
        data.password = btoa(data.password); //Convert to Base64

        try {
            setIsLoading(true);
            const response = await axios.post('/api/v1/identity/login', data);
            console.log(response);
            if (response.status == 200) {
                localStorage.setItem('token', response.data);
                setIsLoading(false);
                authContext.update(true);
                history.push("/");
            }
        }
        catch (e) {
            const response = e.response;
            console.log("Response");
            console.log(response);
            setIsLoading(false);

            if (response !== undefined && response.status == 401) {
                console.log("Wrong creds");
                setCredentialsIncorrect(true);
            }
            else {
                setIsGenericError(true);
            }
        }
    }

    useEffect(() => {
    }, [formState, credentialsIncorrect, isGenericError])

    return (
        <Spin spinning={isLoading}>
            <Form
                name="login"
                className="login-form"
                onFinish={handleSubmit}
            >
                <h2>Login</h2>

                {credentialsIncorrect === true ? <AlertMesage message={msgLogin} /> : null}
                {isGenericError === true ? <AlertMesage message={msgGenericError} /> : null}

                <Form.Item
                    name="username"
                    value={formState.usernameOrEmail}
                    rules={[{ required: true, message: 'Please input your Username!' }]}
                >
                    <Input
                        prefix={<UserOutlined
                            className="site-form-item-icon" />}
                        placeholder="Username"
                        onChange={e => handleChangeFormState(e.target.value, "username")}
                        onPressEnter={handleSubmit}
                    />
                </Form.Item>
                <Form.Item
                    name="password"
                    value={formState.password}
                    rules={[{ required: true, message: 'Please input your Password!' }]}
                >
                    <Input
                        prefix={<LockOutlined className="site-form-item-icon" />}
                        type="password"
                        placeholder="Password"
                        onChange={e => handleChangeFormState(e.target.value, "password")}
                        onPressEnter={handleSubmit}
                    />
                </Form.Item>
                <Form.Item>
                    <Form.Item name="remember" noStyle>
                        <Checkbox defaultChecked={formState.remember} onChange={e => handleChangeFormState(e.target.checked, "remember")}>Remember me</Checkbox>
                    </Form.Item>
                    <Link to="/register" className="login-form-register-link">Register</Link>
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit" className="login-form-button" >
                        Log in
                    </Button>
                </Form.Item>
            </Form>
        </Spin>

    )
};

export default LoginForm;