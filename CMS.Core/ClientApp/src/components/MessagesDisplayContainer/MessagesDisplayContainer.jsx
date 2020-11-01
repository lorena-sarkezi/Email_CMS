import React, {useEffect, useState} from 'react';
import { useParams, Link } from 'react-router-dom';
import { Card, Spin, Button, Col, Row } from 'antd';

import LeftOutlined from '@ant-design/icons/LeftOutlined';
import MailTwoTone from '@ant-design/icons/MailTwoTone';

import MessageDisplayCard from './MessageDisplayCard';

import axios from 'axios';



const LoadingSpinner = () => {
    return(
        <div style={{textAlign:"center"}}>
            <Spin size="large" />
        </div>
    )
}


const MessagesDisplayContainer = (props) =>{

    

    const {threadId} = useParams();

    console.log(threadId);

    const [threadData, setThreadData] = useState(null);
    const [isLoading, setIsLoading] =useState(false);

    const getThreadMessages = async () =>{
        setIsLoading(true);
        const response = await axios.get(`/api/v1/messaging/threads/${threadId}`);
        setIsLoading(false);

        console.log(response.data);
        setThreadData(response.data);
    }

    useEffect(() =>{
        getThreadMessages();
    }, [])

    return(
        <React.Fragment>
            <Row style={{marginBottom:"5px"}}>
                <Col flex="0 1 auto">
                    <Link to="/">
                        <Button tyle="default" icon={<LeftOutlined />}>
                            Back
                        </Button> 
                    </Link>
                </Col>

                <Col flex="auto"/>

                <Col flex="0 1 auto">
                    <Link to="#" >
                        <Button tyle="default" icon={<MailTwoTone />}>
                            Reply
                        </Button> 
                    </Link>
                </Col>

            </Row>
            {
                isLoading === true ? <LoadingSpinner /> : null 
            }
            {
                threadData !== null ? threadData.messages.map((value, index) => <MessageDisplayCard record={value}/>) : null
            }
            
        </React.Fragment>
        
    )
}

export default MessagesDisplayContainer;