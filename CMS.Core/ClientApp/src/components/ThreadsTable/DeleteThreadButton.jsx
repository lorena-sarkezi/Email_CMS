import React from 'react';

import {Button, Popconfirm} from 'antd';
import DeleteOutlined from '@ant-design/icons/DeleteOutlined';

const DeleteThreadButton = (props) => {
    return(
        <Popconfirm
            title="Are you sure?"
            okText="Yes"
            cancelText="No"
        >
            <Button type="danger" icon={<DeleteOutlined />} />
        </Popconfirm>
    )
}

export default DeleteThreadButton;