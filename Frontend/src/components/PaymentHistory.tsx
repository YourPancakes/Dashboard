import { useEffect, useState } from 'react'
import type { PaymentDto } from '../types'
import { getClientPayments, createPayment, deletePayment } from '../api'
import './PaymentHistory.css'

interface Props {
  clientId: number
  onClose: () => void
}

export function PaymentHistory({ clientId, onClose }: Props) {
  const [payments, setPayments] = useState<PaymentDto[]>([])
  const [amount, setAmount] = useState(0)

  const load = async () => {
    const list = await getClientPayments(clientId)
    setPayments(list)
  }

  useEffect(() => {
    load()
  }, [clientId])

  const add = async () => {
    await createPayment({ clientId, amountT: amount, timestamp: new Date().toISOString() })
    setAmount(0)
    await load()
  }

  const remove = async (id: number) => {
    await deletePayment(id)
    await load()
  }
  

  return (
    <div className="overlay">
      <div className="modal">
        <button className="close-btn" onClick={onClose}>&times;</button>
        <h2>Payment History</h2>
        <ul className="payments-list">
          {payments.map(p => (
            <li key={p.id}>
              <span>{new Date(p.timestamp).toLocaleString()}</span>
              <span>{p.amountT}</span>
              <button onClick={() => remove(p.id)}>Delete</button>
            </li>
          ))}
        </ul>
        <div className="add-payment">
          <input
            type="number"
            value={amount}
            onChange={e => setAmount(Number(e.target.value))}
            placeholder="Amount"
          />
          <button onClick={add}>Add Payment</button>
        </div>
      </div>
    </div>
  )
}
