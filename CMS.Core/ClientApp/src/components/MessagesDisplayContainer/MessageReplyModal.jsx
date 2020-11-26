import React, {useEffect, useState} from 'react';
import axios from '../../axios';
import { Input, Modal, Spin, Button } from 'antd';
import { couldStartTrivia, preProcessFile } from 'typescript';

const MessageReplyModal = (props) => {

    const [textAreaContent, setTextAreaContent] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    const {TextArea} = Input;

    const autosize = {
        minRows: 4 
    };

    const handleCancel = () => {
        setTextAreaContent("");
        props.visibleHandler(false);
    }

    const CancelButton = ({cancelHandler}) => {
        return(
            <Button onClick={() => cancelHandler()}>
                Cancel
            </Button>
        )
    }

    const ConfirmButton = ({confirmHandler}) =>  {
        return(
            <Button onClick={confirmHandler} loading={isLoading} type="primary">
                Send
            </Button>
        )
    }

    const handleConfirm = async () => {
        setIsLoading(true);
        const content = {
            threadId: parseInt(props.threadId),
            messageContent: textAreaContent
        };
    
        const requestUri = '/api/v1/messaging/send';
        const response = await axios.post(requestUri, content);

        setIsLoading(false);
        props.dummyStateCallback(!props.dummmyState);
        props.visibleHandler(false);
    }

    return(
        <Modal 
            title="Compose a reply"
            visible={props.visible}
            //destroyOnClose={true}
            footer={[
                <CancelButton cancelHandler={handleCancel}/>,
                <ConfirmButton confirmHandler={handleConfirm}/>
            ]}
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