import { useState, useEffect } from 'react'
import type { ClientDto } from '../types'
import './ClientForm.css'

interface Props {
  client?: ClientDto
  onCancel: () => void
  onSave: (dto: { name: string; email: string; balanceT: number }) => void
}

export function ClientForm({ client, onCancel, onSave }: Props) {
  const [name, setName] = useState(client?.name ?? '')
  const [email, setEmail] = useState(client?.email ?? '')
  const [balance, setBalance] = useState(client?.balanceT.toString() ?? '0')

  useEffect(() => {
    if (client) {
      setName(client.name)
      setEmail(client.email)
      setBalance(client.balanceT.toString())
    }
  }, [client])

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    onSave({ name, email, balanceT: parseFloat(balance) })
  }

  return (
    <div className="modal">
      <h2>{client ? 'Edit Client' : 'New Client'}</h2>
      <form className="client-form" onSubmit={handleSubmit}>
        <label>
          Name
          <input
            type="text"
            value={name}
            onChange={e => setName(e.target.value)}
            required
          />
        </label>
        <label>
          Email
          <input
            type="email"
            value={email}
            onChange={e => setEmail(e.target.value)}
            required
          />
        </label>
        <label>
          Balance
          <input
            type="number"
            step="0.01"
            value={balance}
            onChange={e => setBalance(e.target.value)}
            required
          />
        </label>
        <div className="form-buttons">
          <button type="submit">Save</button>
          <button type="button" onClick={onCancel}>
            Cancel
          </button>
        </div>
      </form>
    </div>
  )
}
