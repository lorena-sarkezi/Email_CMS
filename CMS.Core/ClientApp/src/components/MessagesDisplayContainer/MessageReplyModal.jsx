import React, {useEffect, useState} from 'react';
import axios from 'axios';
import { Input, Modal } from 'antd';
import { preProcessFile } from 'typescript';

const MessageReplyModal = (props) => {

    const [textAreaContent, setTextAreaContent] = useState("");

    const {TextArea} = Input;

    const autosize = {
        minRows: 4 
    };

    const handleCancel = () => {
        setTextAreaContent("");
        props.visibleHandler(false);
    }

    const handleConfirm = async () => {
        const content = {
            threadId: props.threadId,
            message: textAreaContent
        };
        const requestUri = '/api/v1/messaging';
        const response = await axios.post(requestUri, content);
        
    }

    // useEffect(() => {
    //     console.log(textAreaContent);
    // }, []);

    return(
        <Modal 
            title="Compose a reply"
            visible={props.visible}
            onCancel={() => handleCancel()}
            onOk={() => handleConfirm()}
        >
            <TextArea 
                placeholder="Type your message here..."
                autoSize={autosize} 
                value={textAreaContent} 
                onChange={event => setTextAreaContent(event.target.value)} />
        </Modal>
    )
}

export default MessageReplyModal;