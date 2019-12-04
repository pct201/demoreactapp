import React from 'react';
import { Button, Modal } from 'react-bootstrap';

const PreviewPopup = (props) => {
    return (
        <Modal show={props.show} size="lg" aria-labelledby="contained-modal-title-vcenter" centered="true">
            <Modal.Body dangerouslySetInnerHTML={{ __html: props.summernoteContent }}>
                
            </Modal.Body>
            <Modal.Footer>
                <Button onClick={props.popupClose}>Ok</Button>
            </Modal.Footer>
        </Modal>)
}

export default PreviewPopup;