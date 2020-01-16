export class Profile {
    displayName: string;
    forwardingAddress: string;
}

export class Address {
    name: string;
    description?: string;
    emailAddress: string;
    forwardToEmailAddress?: string;
}

export class MaskedEmail extends Address {
    forwardingEnabled: boolean;

    static fromAddress(address: Address): MaskedEmail {

        var masked: MaskedEmail = {
            name: address.name,
            description: address.description,
            emailAddress: address.emailAddress,
            forwardToEmailAddress: address.forwardToEmailAddress,
            forwardingEnabled: (address.forwardToEmailAddress !== undefined && address.forwardToEmailAddress != null)
        };
        return masked;
    }
}

export class MaskedEmailRequest {
    name: string;
    description?: string;
    passwordHash?: string;
    forwardingEnabled: boolean;
}

export class UpdateMaskedEmailRequest{
    name: string;
    description?: string;
}