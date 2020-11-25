import React from 'react'
import {Link} from 'react-router-dom';
import { Card } from 'antd';

const NotFound = () => {
    return (
        <Card>
            <div className='div-text-content-centered'>
                <h2>Page not found!</h2><br />
                <Link to="/">Go to Index Page</Link>
            </div>
        </Card>


    )
}

export default NotFound;