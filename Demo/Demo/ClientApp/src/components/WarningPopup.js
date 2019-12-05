import React from 'react';
import { Button, Modal } from 'react-bootstrap';

const WarningPopup = (props) => {
    return (
        <Modal show={props.show} size="lg" aria-labelledby="contained-modal-title-vcenter" centered="true">
            <Modal.Body dangerouslySetInnerHTML={{ __html: props.message }}>
                
            </Modal.Body>
            <Modal.Footer>                
                <Button onClick={props.popupClose}>OK</Button>
            </Modal.Footer>
        </Modal>)
}

export default WarningPopup;