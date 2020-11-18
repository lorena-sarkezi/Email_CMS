import React, {useState, useEffect} from 'react';
import {Link, useHistory} from 'react-router-dom';
import axios from '../../axios';
import { Form, Input, Button, Checkbox } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { createCompilerHost } from 'typescript';

const initialFormState = {
    emailOrUsername: "",
    password: "",
    remember: false
};

const Login = props => {    

    const history = useHistory();

    const [formState, setFormState] = useState(initialFormState);

    const handleChangeFormState = (value, prop) => {
        let data = {...formState};
        data[prop] = value;
        setFormState(data);
    }

    const handleSubmit = async () =>{

        console.log(axios);

        let data = {...formState};
        data.password = btoa(data.password); //Convert to Base64

        const response = await axios.post('/api/v1/identity/login',data);
        if(response.status === 200){
            localStorage.setItem('token',response.data);
            //axios.defaults.header.common['Authorization'] = `Bearer ${response.data}`;

            history.push("/");
        }
        else{
            alert("An error occured");
        }
    }

    useEffect(() => {
        console.log(formState);
    },[formState])

    return (
        <Form
            name="normal_login"
            className="login-form"
            initialValues={{ remember: true }}
            //onFinish={onFinish}
        >
            <Form.Item
                name="username"
                value={formState.usernameOrEmail}
                rules={[{ required: true, message: 'Please input your Username!' }]}
            >
                <Input 
                    prefix={<UserOutlined 
                    className="site-form-item-icon" />} 
                    placeholder="Username" 
                    onChange={e => handleChangeFormState(e.target.value, "emailOrUsername")}/>
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
                />
            </Form.Item>
            <Form.Item>
                <Form.Item name="remember"  noStyle>
                    <Checkbox defaultChecked={formState.remember} onChange={e => handleChangeFormState(e.target.checked, "remember")}>Remember me</Checkbox>
                </Form.Item>
                <Link to="/register" className="login-form-register-link">Register</Link>
            </Form.Item>

            <Form.Item>
                <Button type="primary" htmlType="button" className="login-form-button" onClick={() => handleSubmit()}>
                    Log in
        </Button>
        
            </Form.Item>
        </Form>
    )
};

export default Login;